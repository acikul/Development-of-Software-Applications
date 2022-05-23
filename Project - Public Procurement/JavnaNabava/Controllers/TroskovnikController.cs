using JavnaNabava.Extensions;
using JavnaNabava.Extensions.Selectors;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    public class TroskovnikController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public TroskovnikController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: TroskovnikController
        public ActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Troskovniks.AsNoTracking();
            int count = query.Count();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };
            if (page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = 1, sort = sort, ascending = ascending });
            }

            query = query.ApplySort(
                sort,
                ascending);

            var troskovnici = query
                                .Select(v => new TroskovnikViewModel
                                {
                                    SifTroskovnik = v.SifTroskovnik,
                                    IspravnoPopunjen = v.IspravnoPopunjen,
                                    ImeValute = v.SifValutaNavigation.ImeValute,
                                    //NazNatjecaja = v.Natjecajs.ElementAt(0)

                                })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToList();

            foreach (TroskovnikViewModel troskovnik in troskovnici)
            {
                troskovnik.Stavke = ctx.Stavkas
                                      .Where(s => s.SifTroskovnik == troskovnik.SifTroskovnik)
                                      .OrderBy(s => s.SifStavke)
                                      .Select(s => new StavkaViewModel
                                      {
                                          SifStavke = s.SifStavke,
                                          NazivStavke = s.NazivStavke,
                                          JedCijena = s.JedCijena,
                                          StopaPdv = s.StopaPdv,
                                          Kolicina = s.Kolicina,
                                          UkCijena = s.UkCijena,
                                          NapomenaPonuditelja = s.NapomenaPonuditelja,
                                          JedCijenaSaPdv = s.JedCijenaSaPdv,
                                          UkCijenaSaPdv = s.UkCijenaSaPdv,
                                          IznosPdv = s.IznosPdv,
                                          NapomenaNarucitelja = s.NapomenaNarucitelja,
                                          SifTroskovnik = s.SifTroskovnik,
                                          NazivVrsteStavke = s.SifVrstaStavkeNavigation.NazivVrsteStavke
                                      }).ToList();
            }

            var model = new TroskovniciViewModel
            {
                Troskovnici = troskovnici,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        // GET: TroskovnikController/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        // POST: TroskovnikController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Troskovnik troskovnik)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(troskovnik);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Troskovnik dodan. Sifra troskovnika = {troskovnik.SifTroskovnik}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(troskovnik);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(troskovnik);
            }
        }

        // GET: TroskovnikController/Edit/5
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var troskovnik = await ctx.Troskovniks
                                  .AsNoTracking()
                                  .Where(m => m.SifTroskovnik == id)
                                  .SingleOrDefaultAsync();
            if (troskovnik != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(troskovnik);
            }
            else
            {
                return NotFound($"Neispravna sifra troskovnika: {id}");
            }
        }

        // POST: TroskovnikController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Troskovnik troskovnik, int page = 1, int sort = 1, bool ascending = true)
        {
            if (troskovnik == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Troskovniks.AnyAsync(m => m.SifTroskovnik == troskovnik.SifTroskovnik);
            if (!checkId)
            {
                return NotFound($"Neispravna sifra troskovnika: {troskovnik?.SifTroskovnik}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(troskovnik);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Troskovnik ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(troskovnik);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(troskovnik);
            }
        }


        // POST: TroskovnikController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int SifTroskovnik, int page = 1, int sort = 1, bool ascending = true)
        {
            var troskovnik = ctx.Troskovniks.Find(SifTroskovnik);
            if (troskovnik != null)
            {
                try
                {
                    int sifraTroskovnika = troskovnik.SifTroskovnik;
                    ctx.Remove(troskovnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Troskovnik {sifraTroskovnika} uspjesno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja troskovnika: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji troskovnik sa šifrom: {SifTroskovnik}";
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }

        private async Task PrepareDropDownLists()
        {
            var sifValute = await ctx.Valuta
                              .OrderBy(d => d.SifValuta)
                              .Select(d => new { d.SifValuta, d.ImeValute })
                              .ToListAsync();


            ViewBag.sifValute = new SelectList(sifValute, nameof(Valutum.SifValuta), nameof(Valutum.ImeValute));
        }


        public async Task<IActionResult> Show(int id, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            var troskovnik = await ctx.Troskovniks
                                    .Where(m => m.SifTroskovnik == id)
                                    .Select(m => new TroskovnikViewModel
                                    {
                                        SifTroskovnik = m.SifTroskovnik,
                                        IspravnoPopunjen = m.IspravnoPopunjen,
                                        ImeValute = m.SifValutaNavigation.ImeValute,
                                    })
                                    .FirstOrDefaultAsync();
            if (troskovnik == null)
            {
                return NotFound($"Troskovnik {id} ne postoji");
            }

            else
            {
                
                var stavke = await ctx.Stavkas
                                      .Where(s => s.SifTroskovnik == troskovnik.SifTroskovnik)
                                      .OrderBy(s => s.SifStavke)
                                      .Select(s => new StavkaViewModel
                                      {
                                          SifStavke = s.SifStavke,
                                          NazivStavke = s.NazivStavke,
                                          JedCijena = s.JedCijena,
                                          StopaPdv = s.StopaPdv,
                                          Kolicina = s.Kolicina,
                                          UkCijena = s.UkCijena,
                                          NapomenaPonuditelja = s.NapomenaPonuditelja,
                                          JedCijenaSaPdv = s.JedCijenaSaPdv,
                                          UkCijenaSaPdv = s.UkCijenaSaPdv,
                                          IznosPdv = s.IznosPdv,
                                          NapomenaNarucitelja = s.NapomenaNarucitelja,
                                          SifTroskovnik = s.SifTroskovnik,
                                          NazivVrsteStavke = s.SifVrstaStavkeNavigation.NazivVrsteStavke
                                      })
                                      .ToListAsync();
                troskovnik.Stavke = stavke;

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;


                return View(viewName, troskovnik);
            }
        }


    }
}
