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
    public class StavkaController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public StavkaController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: StavkaController
        public ActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Stavkas.AsNoTracking();
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

            var Stavke = query
                                .Select(v => new StavkaViewModel
                                {
                                    SifStavke = v.SifStavke,
                                    NazivStavke = v.NazivStavke,
                                    JedCijena = v.JedCijena,
                                    StopaPdv = v.StopaPdv,
                                    Kolicina = v.Kolicina,
                                    UkCijena = v.UkCijena,
                                    NapomenaPonuditelja = v.NapomenaPonuditelja,
                                    JedCijenaSaPdv = v.JedCijenaSaPdv,
                                    UkCijenaSaPdv = v.UkCijenaSaPdv,
                                    IznosPdv = v.IznosPdv,
                                    NapomenaNarucitelja = v.NapomenaNarucitelja,
                                    SifTroskovnik = v.SifTroskovnik,
                                    NazivVrsteStavke = v.SifVrstaStavkeNavigation.NazivVrsteStavke

                                })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToList();
            var model = new StavkeViewModel
            {
                Stavke = Stavke,
                PagingInfo = pagingInfo
            };

            return View(model);
        }


        // GET: StavkaController/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        // POST: StavkaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stavka stavka)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(stavka);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Stavka {stavka.NazivStavke} dodana. Sifra stavke = {stavka.SifStavke}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(stavka);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(stavka);
            }
        }

        // GET: StavkaController/Edit/5
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavka = await ctx.Stavkas
                                  .AsNoTracking()
                                  .Where(m => m.SifStavke == id)
                                  .SingleOrDefaultAsync();
            if (stavka != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(stavka);
            }
            else
            {
                return NotFound($"Neispravna sifra stavke: {id}");
            }
        }

        // POST: StavkaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Stavka stavka, int page = 1, int sort = 1, bool ascending = true)
        {
            if (stavka == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Stavkas.AnyAsync(m => m.SifStavke == stavka.SifStavke);
            if (!checkId)
            {
                return NotFound($"Neispravna sifra stavke: {stavka?.SifStavke}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(stavka);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Stavka ažurirana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(stavka);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(stavka);
            }
        }

        // POST: StavkaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int SifStavke, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavka = ctx.Stavkas.Find(SifStavke);
            if (stavka != null)
            {
                try
                {
                    string ime = stavka.NazivStavke;
                    ctx.Remove(stavka);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Stavka {ime} sa šifrom {SifStavke} obrisana.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja stavke: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji stavka sa šifrom: {SifStavke}";
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }

        private async Task PrepareDropDownLists()
        {
            var vrstaStavke = await ctx.VrstaStavkes
                              .OrderBy(d => d.SifVrsteStavke)
                              .Select(d => new { d.SifVrsteStavke, d.NazivVrsteStavke })
                              .ToListAsync();

            var sifraTroskovnik = await ctx.Troskovniks
                              .OrderBy(d => d.SifTroskovnik)
                              .Select(d => d.SifTroskovnik)
                              .ToListAsync();


            ViewBag.vrstaStavke = new SelectList(vrstaStavke, nameof(VrstaStavke.SifVrsteStavke), nameof(VrstaStavke.NazivVrsteStavke));
            ViewBag.sifraTroskovnik = new SelectList(sifraTroskovnik, nameof(Troskovnik.SifTroskovnik));
        }

    }
}
