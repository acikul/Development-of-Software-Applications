using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JavnaNabava.Extensions;
using JavnaNabava.Extensions.Selectors;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    public class KompetencijaController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly ILogger<KompetencijaController> logger;
        private readonly AppSettings appSettings;

        public KompetencijaController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options, ILogger<KompetencijaController> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
            appSettings = options.Value;
        }

        public IActionResult Index(int page = 1, int sort = 3, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Kompetencijas
                           .AsNoTracking();

            int count = query.Count();
            if (count == 0)
            {
                logger.LogInformation("Ne postoji nijedna kompetencija.");
                TempData[Constants.Message] = "Ne postoji niti jedna kopetencija.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index));
            }

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
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            var kompetencije = query.Select(k => new KompetencijaViewModel
            {
                SifKompetencije = k.SifKompetencije,
                DetaljiKompetencije = k.DetaljiKompetencije,
                Oibstrucnjaka = k.Oibstrucnjaka,
                NazivVrsteKompetencije = k.SifVrsteKompetencijeNavigation.NazivVrsteKompetencije
            })
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

            var model = new KompetencijeViewModel
            {
                Kompetencije = kompetencije,
                PagingInfo = pagingInfo
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Kompetencija kompetencija)
        {
            logger.LogTrace(JsonSerializer.Serialize(kompetencija));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(kompetencija);
                    await ctx.SaveChangesAsync();
                    logger.LogInformation(new EventId(1000), $"Kompetencija {kompetencija.DetaljiKompetencije} dodana.");

                    TempData[Constants.Message] = $"Kompetencija {kompetencija.DetaljiKompetencije} dodana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove kompetencije: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(kompetencija);
                }
            }
            else
            {
                return View(kompetencija);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int sifKompetencije, int page = 1, int sort = 3, bool ascending = true)
        {
            var kompetencija = ctx.Kompetencijas.Find(sifKompetencije);
            if (kompetencija != null)
            {
                try
                {
                    string naziv = kompetencija.DetaljiKompetencije;
                    ctx.Remove(kompetencija);
                    ctx.SaveChanges();
                    logger.LogInformation($"Kompetencija {naziv} uspješno obrisana");
                    TempData[Constants.Message] = $"Kompetencija {naziv} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja kompetencije: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                    logger.LogError("Pogreška prilikom brisanja kompetencije: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning("Ne postoji kompetencija sa šifrom: {0} ", sifKompetencije);
                TempData[Constants.Message] = "Ne postoji kompetencija sa šifrom: " + sifKompetencije;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 3, bool ascending = true)
        {
            var kompetencija = await ctx.Kompetencijas
                                  .AsNoTracking()
                                  .Where(m => m.SifKompetencije == id)
                                  .SingleOrDefaultAsync();
            if (kompetencija != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(kompetencija);
            }
            else
            {
                return NotFound($"Neispravna šifra kompetencije: {id}");
            }
        }

        private async Task PrepareDropDownLists()
        {
            var oibovi = await ctx.Strucnjaks
                             .Select(d => d.Oibstrucnjaka)
                             .ToListAsync();
            var vrsteKompetencija = await ctx.VrstaKompetencijes
                                  .OrderBy(d => d.SifVrsteKompetencije)
                                  .Select(d => new { d.SifVrsteKompetencije, d.NazivVrsteKompetencije })
                                  .ToListAsync();

            ViewBag.Oibovi = new SelectList(oibovi, nameof(Strucnjak.Oibstrucnjaka));
            ViewBag.VrsteKompetencija = new SelectList(vrsteKompetencija, nameof(VrstaKompetencije.SifVrsteKompetencije), nameof(VrstaKompetencije.NazivVrsteKompetencije));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Kompetencija kompetencija, int page = 1, int sort = 3, bool ascending = true)
        {
            if (kompetencija == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Kompetencijas.AnyAsync(m => m.SifKompetencije == kompetencija.SifKompetencije);
            if (!checkId)
            {
                return NotFound($"Neispravna sifra kompetencija: {kompetencija?.SifKompetencije}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(kompetencija);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Kompetencija ažurirana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(kompetencija);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(kompetencija);
            }
        }
    }
}
