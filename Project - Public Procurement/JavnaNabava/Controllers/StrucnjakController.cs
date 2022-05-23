using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JavnaNabava.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions;
using Microsoft.EntityFrameworkCore;
using JavnaNabava.ViewModels;
using JavnaNabava.Extensions;
using JavnaNabava.Extensions.Selectors;

namespace JavnaNabava.Controllers
{
    public class StrucnjakController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appData;

        public StrucnjakController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public async Task<IActionResult> Index(string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.vw_Strucnjaci.AsQueryable();

            StrucnjaciFilter sf = StrucnjaciFilter.FromString(filter);
            #region Apply filter

            if (!sf.IsEmpty())
            {
                if (sf.SifStrucneSpreme.HasValue)
                {
                    sf.NazivStrucneSpreme = await ctx.vw_Strucnjaci
                                              .Where(p => p.SifStrucneSpreme == sf.SifStrucneSpreme)
                                              .Select(vp => vp.NazivStrucneSpreme)
                                              .FirstOrDefaultAsync();
                }
                if (sf.SifGrada.HasValue)
                {
                    sf.NazivGrada = await ctx.vw_Strucnjaci
                                        .Where(p => p.SifGrada == sf.SifGrada)
                                        .Select(vp => vp.NazivGrada)
                                        .FirstOrDefaultAsync();
                }
                query = sf.Apply(query);
            }
            #endregion

            int count = await query.CountAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (count > 0 && (page < 1 || page > pagingInfo.TotalPages))
            {
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending, filter });
            }

            query = query.ApplySort(sort, ascending);

            var strucnjaci = await query 
                                  .Skip((page - 1) * pagesize)
                                  .Take(pagesize)
                                  .ToListAsync();

            for (int i = 0; i < strucnjaci.Count; i++)
            {
                strucnjaci[i].Position = (page - 1) * pagesize + i;

                //dodavanje kompetencija svakom
                var komps = await ctx.Kompetencijas.Where(d => d.Oibstrucnjaka == strucnjaci[i].Oibstrucnjaka).Select(d => d.DetaljiKompetencije).ToListAsync();
                string temp = "";
                bool prvi = true;
                foreach (var kompa in komps)
                {
                    if (prvi) { 
                        temp = kompa;
                        prvi = false;
                    } else
                    {
                        temp = temp + ", " + kompa;
                    }
                }
                strucnjaci[i].Komps = temp;
            }
            var model = new StrucnjaciViewModel
            {
                Strucnjaci = strucnjaci,
                PagingInfo = pagingInfo,
                Filter = sf
            };

            return View(model);
        }
        
        [HttpPost]
        public IActionResult Filter(StrucnjaciFilter filter)
        {
            return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
        }

        public async Task<IActionResult> Show(string id, int? position, string filter, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            var strucnjak = await ctx.Strucnjaks
                                    .Where(d => d.Oibstrucnjaka == id)
                                    .Select(d => new StrucnjakViewModel
                                    {
                                        Oibstrucnjaka = d.Oibstrucnjaka,
                                        ImeStrucnjaka = d.ImeStrucnjaka,
                                        PrezimeStrucnjaka = d.PrezimeStrucnjaka,
                                        EmailStrucnjaka = d.EmailStucnjaka,
                                        BrojMobitelaStrucnjaka = d.BrojMobitelaStrucnjaka,
                                        Oibponuditelja = d.Oibponuditelja,
                                        SifStrucneSpreme = d.SifStrucneSpreme,
                                        SifGrada = d.SifGrada
                                    })
                                    .FirstOrDefaultAsync();
            if (strucnjak == null)
            {
                return NotFound($"Strucnjak s OIB-om {id} ne postoji");
            }
            else
            {
                strucnjak.NazivPonuditelja = await ctx.Ponuditeljs
                                                .Where(p => p.Oibponuditelja == strucnjak.Oibponuditelja)
                                                .Select(p => p.NazivPonuditelja)
                                                .FirstOrDefaultAsync();
                strucnjak.NazivGrada = await ctx.Grads
                                            .Where(p => p.SifGrada == strucnjak.SifGrada)
                                            .Select(p => p.NazivGrada)
                                            .FirstOrDefaultAsync();
                strucnjak.NazivStrucneSpreme = await ctx.StrucnaSpremas
                                                    .Where(p => p.SifStrucneSpreme == strucnjak.SifStrucneSpreme)
                                                    .Select(p => p.NazivStrucneSpreme)
                                                    .FirstOrDefaultAsync();

                var kompetencije = await ctx.Kompetencijas
                                      .Where(s => s.Oibstrucnjaka == strucnjak.Oibstrucnjaka)
                                      .OrderBy(s => s.Oibstrucnjaka)
                                      .Select(s => new KompetencijaViewModel
                                      {
                                          SifKompetencije = s.SifKompetencije,
                                          DetaljiKompetencije = s.DetaljiKompetencije,
                                          NazivVrsteKompetencije = s.SifVrsteKompetencijeNavigation.NazivVrsteKompetencije,
                                          SifVrsteKompetencije = s.SifVrsteKompetencije,
                                          Oibstrucnjaka = s.Oibstrucnjaka
                                      })
                                      .ToListAsync();
                strucnjak.Kompetencije = kompetencije;

                if (position.HasValue)
                {
                    page = 1 + position.Value / appData.PageSize;
                    await SetPreviousAndNext(position.Value, filter, sort, ascending);
                }

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                ViewBag.Filter = filter;
                ViewBag.Position = position;

                return View(viewName, strucnjak);
            }
        }

        private async Task SetPreviousAndNext(int position, string filter, int sort, bool ascending)
        {
            var query = ctx.vw_Strucnjaci.AsQueryable();

            StrucnjaciFilter sf = new StrucnjaciFilter();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                sf = StrucnjaciFilter.FromString(filter);
                if (!sf.IsEmpty())
                {
                    query = sf.Apply(query);
                }
            }

            query = query.ApplySort(sort, ascending);

            if (position > 0)
            {
                ViewBag.Previous = await query.Skip(position - 1).Select(d => d.Oibstrucnjaka).FirstAsync();
            }
            if (position < await query.CountAsync() - 1)
            {
                ViewBag.Next = await query.Skip(position + 1).Select(d => d.Oibstrucnjaka).FirstAsync();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //await PrepareDropDownLists();
            var strucnjak = new StrucnjakViewModel();
            return View(strucnjak);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StrucnjakViewModel model)
        {
            if (ModelState.IsValid)
            {
                Strucnjak s = new Strucnjak();
                s.Oibstrucnjaka = model.Oibstrucnjaka;
                s.ImeStrucnjaka = model.ImeStrucnjaka;
                s.PrezimeStrucnjaka = model.PrezimeStrucnjaka;
                s.EmailStucnjaka = model.EmailStrucnjaka;
                s.BrojMobitelaStrucnjaka = model.BrojMobitelaStrucnjaka;
                s.Oibponuditelja = model.Oibponuditelja;
                s.SifStrucneSpreme = model.SifStrucneSpreme;
                s.SifGrada = model.SifGrada;

                foreach (var kompa in model.Kompetencije)
                {
                    //dodaj nove
                    var novaKompetencija = new Kompetencija(); // potpuno nova
                    novaKompetencija = new Kompetencija();
                    s.Kompetencijes.Add(novaKompetencija);
                    novaKompetencija.SifVrsteKompetencije = kompa.SifVrsteKompetencije;
                    novaKompetencija.DetaljiKompetencije = kompa.DetaljiKompetencije;
                }

                try
                {
                    ctx.Add(s);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Strucnjak uspješno dodan. OIB={s.Oibstrucnjaka}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Show), new { id = s.Oibstrucnjaka });
                }
                catch(Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Oibstrucnjaka, string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            var strucnjak = await ctx.Strucnjaks
                                    .Where(d => d.Oibstrucnjaka == Oibstrucnjaka)
                                    .SingleOrDefaultAsync();
            if (strucnjak != null)
            {
                try
                {
                    ctx.Remove(strucnjak);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Stručnjak s OIB-om: {strucnjak.Oibstrucnjaka} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja stručnjaka: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji stručnjak s OIB-om: " + Oibstrucnjaka;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { filter, page, sort, ascending });
        }

        private async Task PrepareDropDownLists()
        {
            var vrsteKompetencija = await ctx.VrstaKompetencijes
                              .Select(d => new { d.SifVrsteKompetencije, d.NazivVrsteKompetencije })
                              .ToListAsync();
            var strucneSpreme = await ctx.StrucnaSpremas
                                  .OrderBy(d => d.SifStrucneSpreme)
                                  .Select(d => new { d.SifStrucneSpreme, d.NazivStrucneSpreme })
                                  .ToListAsync();
            var ponuditelji = await ctx.Ponuditeljs
                                  .OrderBy(d => d.Oibponuditelja)
                                  .Select(d => new { d.Oibponuditelja, d.NazivPonuditelja })
                                  .ToListAsync();
            var gradovi = await ctx.Grads
                                  .OrderBy(d => d.SifGrada)
                                  .Select(d => new { d.SifGrada, d.NazivGrada })
                                  .ToListAsync();

            ViewBag.StrucneSpreme = new SelectList(strucneSpreme, nameof(StrucnaSprema.SifStrucneSpreme), nameof(StrucnaSprema.NazivStrucneSpreme));
            ViewBag.VrsteKompetencija = new SelectList(vrsteKompetencija, nameof(VrstaKompetencije.SifVrsteKompetencije), nameof(VrstaKompetencije.NazivVrsteKompetencije));
            ViewBag.Ponuditelji = new SelectList(ponuditelji, nameof(Ponuditelj.Oibponuditelja), nameof(Ponuditelj.NazivPonuditelja));
            ViewBag.Gradovi = new SelectList(gradovi, nameof(Grad.SifGrada), nameof(Grad.NazivGrada));

        }

        [HttpGet]
        public async Task<IActionResult> deprecatedEdit(string id, int page = 1, int sort = 1, bool ascending = true)
        {
            var strucnjak = await ctx.Strucnjaks
                                  .AsNoTracking()
                                  .Where(m => m.Oibstrucnjaka == id)
                                  .SingleOrDefaultAsync();
            if (strucnjak != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(strucnjak);
            }
            else
            {
                return NotFound($"Neispravna šifra prijave: {id}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deprecatedEdit(Strucnjak strucnjak, int page = 1, int sort = 1, bool ascending = true)
        {
            if (strucnjak == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Strucnjaks.AnyAsync(m => m.Oibstrucnjaka == strucnjak.Oibstrucnjaka);
            if (!checkId)
            {
                return NotFound($"Neispravan OIB: {strucnjak?.Oibstrucnjaka}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(strucnjak);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Strucnjak ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(strucnjak);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(strucnjak);
            }
        }

        [HttpGet]
        public Task<IActionResult> Edit(string id, int? position, string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            return Show(id, position, filter, page, sort, ascending, viewName: nameof(Edit));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StrucnjakViewModel model, int? position, string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            ViewBag.Filter = filter;
            ViewBag.Position = position;

            if (ModelState.IsValid)
            {
                var strucnjak = await ctx.Strucnjaks
                                    .Include(d => d.Kompetencijes)
                                    .Where(d => d.Oibstrucnjaka == model.Oibstrucnjaka)
                                    .FirstOrDefaultAsync();
                if (strucnjak == null)
                {
                    return NotFound("Ne postoji strucnjak s oib-om: " + model.Oibstrucnjaka);
                }
                else
                {

                    strucnjak.ImeStrucnjaka = model.ImeStrucnjaka;
                    strucnjak.PrezimeStrucnjaka = model.PrezimeStrucnjaka;
                    strucnjak.BrojMobitelaStrucnjaka = model.BrojMobitelaStrucnjaka;
                    strucnjak.EmailStucnjaka = model.EmailStrucnjaka;
                    strucnjak.SifGrada = model.SifGrada;
                    strucnjak.Oibponuditelja = model.Oibponuditelja;
                    strucnjak.SifStrucneSpreme = model.SifStrucneSpreme;
                }

                if (position.HasValue)
                {
                    page = 1 + position.Value / appData.PageSize;
                    await SetPreviousAndNext(position.Value, filter, sort, ascending);
                }

                List<int> idKompetencija = model.Kompetencije
                                          .Where(s => s.SifKompetencije > 0)
                                          .Select(s => s.SifKompetencije)
                                          .ToList();
                //izbaci sve koje su nisu više u modelu
                ctx.RemoveRange(strucnjak.Kompetencijes.Where(s => !idKompetencija.Contains(s.SifKompetencije)));

                foreach (var kompa in model.Kompetencije)
                {
                    //ažuriraj postojeće i dodaj nove
                    var novaKompetencija = new Kompetencija(); // potpuno nova ili dohvaćena ona koju treba izmijeniti
                    if (kompa.SifKompetencije > 0)
                    {
                        novaKompetencija = strucnjak.Kompetencijes.First(s => s.SifKompetencije == kompa.SifKompetencije);
                        novaKompetencija.DetaljiKompetencije = kompa.DetaljiKompetencije;
                    }
                    else
                    {
                        novaKompetencija = new Kompetencija();
                        strucnjak.Kompetencijes.Add(novaKompetencija);
                        novaKompetencija.SifVrsteKompetencije = kompa.SifVrsteKompetencije;
                        novaKompetencija.DetaljiKompetencije = kompa.DetaljiKompetencije;
                    }
                   /* novaKompetencija.SifKompetencije = kompa.SifKompetencije;
                    novaKompetencija.Oibstrucnjaka = kompa.Oibstrucnjaka;
                    novaKompetencija.SifVrsteKompetencije = kompa.SifVrsteKompetencije;
                    novaKompetencija.DetaljiKompetencije = kompa.DetaljiKompetencije;*/
                }

                try
                {
                    //ctx.Update(strucnjak);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Strucnjak s OIB-om: {strucnjak.Oibstrucnjaka} uspješno ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = strucnjak.Oibstrucnjaka,
                        position,
                        filter,
                        page,
                        sort,
                        ascending
                    });

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
