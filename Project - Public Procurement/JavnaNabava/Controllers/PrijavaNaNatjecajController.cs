using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Extensions;
using JavnaNabava.Extensions.Selectors;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JavnaNabava.Controllers
{
    public class PrijavaNaNatjecajController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public PrijavaNaNatjecajController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: PrijavaNaNatjecaj
        public async Task<IActionResult> Index(string filter,int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.vw_PrijaveNaNatjecaj.AsQueryable();

            #region Apply filter
            PrijavaNaNatjecajFilter pf = PrijavaNaNatjecajFilter.FromString(filter);
            if (!pf.IsEmpty())
            {
                if (pf.sifNatjecaj.HasValue)
                {
                    pf.NazivNatjecaja = await ctx.vw_PrijaveNaNatjecaj
                                              .Where(p => p.SifNatjecaja == pf.sifNatjecaj)
                                              .Select(vp => vp.NazivNatjecaja)
                                              .FirstOrDefaultAsync();
                }
                query = pf.Apply(query);
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

            var prijave = await query
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToListAsync();

            for (int i = 0; i < prijave.Count; i++)
            {
                prijave[i].Position = (page - 1) * pagesize + i;
            }
            

            var model = new PrijaveNaNatjecajViewModel
            {
                Prijave = prijave,
                PagingInfo = pagingInfo,
                Filter = pf
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Filter(PrijavaNaNatjecajFilter filter)
        {
            return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
        }

        // GET: PrijavaNaNatjecaj/Create
        public async Task<IActionResult> Create()
        {
            var prijava = new PrijavaNaNatjecajViewModel();
            await PrepareDropDownLists();
            return View(prijava);
        }

        // POST: PrijavaNaNatjecaj/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PrijavaNaNatjecajViewModel prijava)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PrijavaNaNatjecaj nova = new PrijavaNaNatjecaj();
                    nova.DatumPrijave = DateTime.Now;
                    
                    if(prijava.VrstaPonuditelja=="fizicka osoba")
                    {
                        nova.Oibkonzorcija = null;
                        nova.Oibponuditelja = prijava.OIBPonuditelj;
                    }
                    else
                    {
                        nova.Oibponuditelja = null;
                        nova.Oibkonzorcija = prijava.OIBKonzorcij;
                    }
                    nova.VrstaPonuditelja = prijava.VrstaPonuditelja;
                    nova.SifNatjecaja = prijava.SifNatjecaja;
                    nova.SifStatusa = prijava.SifStatus;
                    nova.SifTroskovnika = prijava.SifTroskovnika;
                    foreach(var dokument in prijava.Dokumenti)
                    {
                        SluzbeniDokument novidokument = new SluzbeniDokument();
                        novidokument.ImeDokumenta = dokument.ImeDokumenta;
                        novidokument.KlasaDokumenta = dokument.KlasaDokumenta;
                        novidokument.SifVrsteDokumenta = dokument.sifVrsteDokumenta;
                        novidokument.TekstDokumenta = dokument.TekstDokumenta;
                        novidokument.UrudzbeniBroj = dokument.UrudzbeniBroj;
                        nova.SluzbeniDokuments.Add(novidokument);
                    }
                    ctx.Add(nova);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Prijava dodana. Id prijave = {nova.SifPrijava}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new { id = nova.SifPrijava });

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(prijava);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(prijava);
            }
        }

        // GET: PrijavaNaNatjecaj/Edit/5
        public async Task<IActionResult> Edit(int id, int? position, string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            //var prijava = await ctx.PrijavaNaNatjecajs
            //                      .AsNoTracking()
            //                      .Where(m => m.SifPrijava == id)
            //                      .SingleOrDefaultAsync();
            //if (prijava != null)
            //{
            //    ViewBag.Page = page;
            //    ViewBag.Sort = sort;
            //    ViewBag.Ascending = ascending;
            //    await PrepareDropDownLists();
            //    return View(prijava);
            //}
            //else
            //{
            //    return NotFound($"Neispravna šifra prijave: {id}");
            //}
            await PrepareDropDownLists();
            return await Show(id, position, filter, page, sort, ascending, viewName: nameof(Edit));
        }

        // POST: PrijavaNaNatjecaj/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrijavaNaNatjecajViewModel model, int? position, string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            ViewBag.Filter = filter;
            ViewBag.Position = position;

            if (ModelState.IsValid)
            {
                Console.Write("U editu");
                var prijava = await ctx.PrijavaNaNatjecajs
                                .Include(d => d.SluzbeniDokuments)
                                .Where(d => d.SifPrijava == model.SifPrijava)
                                .FirstOrDefaultAsync();
                if (prijava == null)
                {
                    return NotFound("Ne postoji prijava s id-om: " + model.SifPrijava);
                }
                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, filter, sort, ascending);
                }
                prijava.DatumPrijave = model.DatumPrijave;
                prijava.SifNatjecaja = model.SifNatjecaja;
                prijava.SifStatusa = model.SifStatus;
                prijava.VrstaPonuditelja = model.VrstaPonuditelja;
                prijava.SifTroskovnika = model.SifTroskovnika;
                if(model.VrstaPonuditelja == "konzorcij")
                {
                    prijava.Oibkonzorcija = model.OIBKonzorcij;
                    prijava.Oibponuditelja = null;
                }
                else
                {
                    prijava.Oibkonzorcija = null;
                    prijava.Oibponuditelja = model.OIBPonuditelj;
                }
                List<int> idDokumenta = model.Dokumenti
                                 .Where(s => s.SifDokumenta > 0)
                                 .Select(s => s.SifDokumenta)
                                 .ToList();
                ctx.RemoveRange(prijava.SluzbeniDokuments.Where(s => !idDokumenta.Contains(s.SifDokumenta)));

                foreach (var dokument in model.Dokumenti)
                {
                    SluzbeniDokument noviDokument;
                    if(dokument.SifDokumenta > 0)
                    {
                        Console.Write("\n" + "staridokumenti " + dokument.SifDokumenta);
                        noviDokument = prijava.SluzbeniDokuments.First(d => d.SifDokumenta == dokument.SifDokumenta);
                    }
                    else
                    {
                        
                        noviDokument = new SluzbeniDokument();
                        Console.Write("\n" + "novi dokument " + noviDokument.SifDokumenta);
                        prijava.SluzbeniDokuments.Add(noviDokument);
                    }
                    noviDokument.ImeDokumenta = dokument.ImeDokumenta;
                    noviDokument.KlasaDokumenta = dokument.KlasaDokumenta;
                    noviDokument.SifVrsteDokumenta = dokument.sifVrsteDokumenta;
                    noviDokument.UrudzbeniBroj = dokument.UrudzbeniBroj;
                    noviDokument.TekstDokumenta = dokument.TekstDokumenta;
                }
                try
                {
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Prijava ažurirana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = prijava.SifPrijava,
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
                    await PrepareDropDownLists();
                    return View(model);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(model);
            }
        }



        // POST: PrijavaNaNatjecaj/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int SifPrijave,string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            var prijava = ctx.PrijavaNaNatjecajs.Find(SifPrijave);
            if (prijava != null)
            {
                try
                {
                    int sifra = prijava.SifPrijava;
                    ctx.Remove(prijava);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Prijava {sifra} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja prijave: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji prijava sa šifrom: " + SifPrijave;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { filter, page, sort, ascending });
        }

        public async Task<IActionResult> Show(int id, int? position, string filter, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            var prijava = await ctx.PrijavaNaNatjecajs
                                    .Where(m => m.SifPrijava == id)
                                    .Select(m => new PrijavaNaNatjecajViewModel
                                    {
                                        SifPrijava = m.SifPrijava,
                                        VrstaPonuditelja = m.VrstaPonuditelja,
                                        DatumPrijave = m.DatumPrijave,
                                        NazivNatjecaja = m.SifNatjecajaNavigation.NazivNatjecaja,
                                        SifTroskovnika = m.SifTroskovnika,
                                        SifStatus=m.SifStatusa,
                                        SifNatjecaja=m.SifNatjecaja,
                                        Status = m.SifStatusaNavigation.NazivStatusa,
                                        OIBPonuditelj=m.Oibponuditelja,
                                        OIBKonzorcij=m.Oibkonzorcija
                                    })
                                    .FirstOrDefaultAsync();
            if (prijava == null)
            {
                return NotFound($"Prijava {id} ne postoji");
            }
            else
            {
                prijava.Ponuditelj=await ctx.Ponuditeljs.Where(p => p.Oibponuditelja == prijava.OIBPonuditelj)
                                        .Select(p => p.NazivPonuditelja)
                                        .FirstOrDefaultAsync();

                prijava.Konzorcij = await ctx.Konzorcijs.Where(p => p.Oibkonzorcija == prijava.OIBKonzorcij)
                                        .Select(p => p.NazivKonzorcija)
                                        .FirstOrDefaultAsync();

                prijava.NazivNatjecaja = await ctx.Natjecajs.Where(p => p.SifNatjecaja == prijava.SifNatjecaja).
                                        Select(p => p.NazivNatjecaja).FirstOrDefaultAsync();

                var dokumenti = await ctx.SluzbeniDokuments
                                      .Where(s => s.SifPrijave == prijava.SifPrijava)
                                      .OrderBy(s => s.SifDokumenta)
                                      .Select(s => new SluzbeniDokumentViewModel
                                      {
                                          SifDokumenta = s.SifDokumenta,
                                          KlasaDokumenta = s.KlasaDokumenta,
                                          ImeDokumenta = s.ImeDokumenta,
                                          UrudzbeniBroj = s.UrudzbeniBroj,
                                          TekstDokumenta = s.TekstDokumenta,
                                          VrstaDokumenta = s.SifVrsteDokumentaNavigation.NazivVrsteDokumenta,
                                          sifVrsteDokumenta=s.SifVrsteDokumenta,
                                          SifPrijave = s.SifPrijave
                                      })
                                      .ToListAsync();
                prijava.Dokumenti = dokumenti;

                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, filter, sort, ascending);
                }

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                ViewBag.Filter = filter;
                ViewBag.Position = position;


                return View(viewName, prijava);
            }
        }

        private async Task SetPreviousAndNext(int position, string filter, int sort, bool ascending)
        {
            var query = ctx.vw_PrijaveNaNatjecaj.AsQueryable();

            PrijavaNaNatjecajFilter pf = new PrijavaNaNatjecajFilter();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                pf = PrijavaNaNatjecajFilter.FromString(filter);
                if (!pf.IsEmpty())
                {
                    query = pf.Apply(query);
                }
            }

            query = query.ApplySort(sort, ascending);
            if (position > 0)
            {
                ViewBag.Previous = await query.Skip(position - 1).Select(d => d.SifPrijava).FirstAsync();
            }
            if (position < await query.CountAsync() - 1)
            {
                ViewBag.Next = await query.Skip(position + 1).Select(d => d.SifPrijava).FirstAsync();
            }
        }

        private async Task PrepareDropDownLists()
        {

            var osobe = await ctx.Ponuditeljs
                                  .OrderBy(d => d.Oibponuditelja)
                                  .Select(d => new { d.NazivPonuditelja, d.Oibponuditelja })
                                  .ToListAsync();
            var konzorciji = await ctx.Konzorcijs
                                 .OrderBy(d => d.Oibkonzorcija)
                                 .Select(d => new { d.NazivKonzorcija, d.Oibkonzorcija })
                                 .ToListAsync();

            var statusi = await ctx.StatusPrijaves
                                 .OrderBy(d => d.SifStatusa)
                                 .Select(d => new { d.NazivStatusa, d.SifStatusa })
                                 .ToListAsync();
            var natjecaji = await ctx.Natjecajs
                                 .OrderBy(d => d.SifNatjecaja)
                                 .Select(d => new { d.NazivNatjecaja, d.SifNatjecaja })
                                  .ToListAsync();
            var troskovnici = await ctx.Troskovniks
                                 .OrderBy(d => d.SifTroskovnik)
                                 .Select(d => d.SifTroskovnik)
                                  .ToListAsync();
            var vrste = await ctx.VrstaDokumenta
                                 .OrderBy(d => d.SifVrsteDokumenta)
                                 .Select(d => new { d.NazivVrsteDokumenta, d.SifVrsteDokumenta })
                                  .ToListAsync();

            ViewBag.osobe = new SelectList(osobe, nameof(Ponuditelj.Oibponuditelja), nameof(Ponuditelj.NazivPonuditelja));
            ViewBag.konzorciji = new SelectList(konzorciji, nameof(Konzorcij.Oibkonzorcija), nameof(Konzorcij.NazivKonzorcija));
            ViewBag.statusi = new SelectList(statusi, nameof(StatusPrijave.SifStatusa),nameof(StatusPrijave.NazivStatusa));
            ViewBag.natjecaji = new SelectList(natjecaji, nameof(Natjecaj.SifNatjecaja), nameof(Natjecaj.NazivNatjecaja));
            ViewBag.troskovnici = new SelectList(troskovnici, nameof(Troskovnik.SifTroskovnik));
            ViewBag.vrste = new SelectList(vrste, nameof(VrstaDokumentum.SifVrsteDokumenta), nameof(VrstaDokumentum.NazivVrsteDokumenta));
        }
    }
}