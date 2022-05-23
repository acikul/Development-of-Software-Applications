using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Extensions;
using JavnaNabava.Extensions.Selectors;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace JavnaNabava.Controllers
{
    public class KriterijController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;
        public KriterijController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }


        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Kriterijs.AsNoTracking();
            int count = await query.CountAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };
            if (page < 1 || page > pagingInfo.TotalPages && count != 0)
            {
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);


            var Kriteriji = await query.Select(a => new KriterijViewModel
            {
                SifKriterija = a.SifKriterija,
                NazivKriterija = a.NazivKriterija
            })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToListAsync();

            var model = new KriterijiViewModel
            {
                Kriteriji = Kriteriji,
                PagingInfo = pagingInfo
            };
            return View(model);

        }

        public async Task<IActionResult> JTable()
        {

            return View(nameof(JTable));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kriterij = await ctx.Kriterijs
                .FirstOrDefaultAsync(m => m.SifKriterija == id);
            if (kriterij == null)
            {
                return NotFound();
            }

            return View(kriterij);
        }

        public IActionResult Create(int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifKriterija,NazivKriterija")] Kriterij kriterij, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(kriterij);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Kriterij {kriterij.NazivKriterija} dodan. Sifra Kriterija = {kriterij.SifKriterija}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page = page, sort, ascending });
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Neuspjelo dodavanje Kriterija {exc.CompleteExceptionMessage()}";
                    TempData[Constants.ErrorOccurred] = true;
                    return View(kriterij);
                }
            }
            TempData[Constants.Message] = $"Neuspjelo dodavanje Kriterija";
            TempData[Constants.ErrorOccurred] = true;
            return View(kriterij);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var kriterij = await ctx.Kriterijs
                                  .AsNoTracking()
                                  .Where(m => m.SifKriterija == id)
                                  .SingleOrDefaultAsync();
            if (kriterij != null)
            {
                return PartialView(kriterij);
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji kriterij sa šifrom: {id}";
                TempData[Constants.ErrorOccurred] = true;
                return NotFound($"Neispravan id Kriterija: {id}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kriterij kriterij)
        {
            if (kriterij == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Kriterijs.AnyAsync(m => m.SifKriterija == kriterij.SifKriterija);
            if (!checkId)
            {
                return NotFound($"Neispravan id Kriterija: {kriterij?.SifKriterija}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(kriterij);
                    await ctx.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);

                    return PartialView(kriterij);
                }
            }
            else
            {

                return PartialView(kriterij);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var kriterij = await ctx.Kriterijs
                                  .Where(m => m.SifKriterija == id)
                                  .Select(a => new KriterijViewModel
                                  {
                                      SifKriterija = a.SifKriterija,
                                      NazivKriterija = a.NazivKriterija

                                  })
                                  .SingleOrDefaultAsync();
            if (kriterij != null)
            {
                return PartialView(kriterij);
            }
            else
            {
                return NotFound($"Neispravan id Kriterija: {id}");
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kriterij = await ctx.Kriterijs
                .FirstOrDefaultAsync(m => m.SifKriterija == id);
            if (kriterij == null)
            {
                return NotFound();
            }

            return View(kriterij);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var kriterij = await ctx.Kriterijs.FindAsync(id);
            if (kriterij != null)
            {
                try
                {
                    string naziv = kriterij.NazivKriterija;
                    ctx.Remove(kriterij);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Kriterij {naziv} sa šifrom {id} obrisan.",
                        successful = true
                    };

                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = "Pogreška prilikom brisanja Kriterija: " + exc.CompleteExceptionMessage(),
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                var result = new
                {
                    message = $"Kriterij sa šifrom {id} ne postoji",
                    successful = false
                };
                return Json(result);

            }
        }
    }

}
