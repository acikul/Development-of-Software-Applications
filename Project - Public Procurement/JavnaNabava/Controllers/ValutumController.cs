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
    public class ValutumController : Controller
    {

        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public ValutumController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: ValutumController
        public ActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Valuta.AsNoTracking();
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

            var Valutumi = query
                                .Select(v => new ValutumViewModel
                                {
                                    SifValuta = v.SifValuta,
                                    ImeValute = v.ImeValute,
                                    OznValute = v.OznValute

                                })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToList();
            var model = new ValutumiViewModel
            {
                Valutumi = Valutumi,
                PagingInfo = pagingInfo
            };

            return View(model);
        }


        // GET: ValutumController/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        // POST: ValutumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Valutum valutum)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(valutum);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Valuta {valutum.ImeValute} dodana. Sifra valute = {valutum.SifValuta}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(valutum);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(valutum);
            }
        }

        // GET: ValutumController/Edit/5
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var valutum = await ctx.Valuta
                                  .AsNoTracking()
                                  .Where(m => m.SifValuta == id)
                                  .SingleOrDefaultAsync();
            if (valutum != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(valutum);
            }
            else
            {
                return NotFound($"Neispravna sifra valute: {id}");
            }
        }

        // POST: ValutumController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Valutum valutum, int page = 1, int sort = 1, bool ascending = true)
        {
            if (valutum == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Valuta.AnyAsync(m => m.SifValuta == valutum.SifValuta);
            if (!checkId)
            {
                return NotFound($"Neispravna sifra valute: {valutum?.SifValuta}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(valutum);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Valuta ažurirana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(valutum);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(valutum);
            }
        }


        // POST: ValutumController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int SifValuta, int page = 1, int sort = 1, bool ascending = true)
        {
            var valutum = ctx.Valuta.Find(SifValuta);
            if (valutum != null)
            {
                try
                {
                    string ime = valutum.ImeValute;
                    ctx.Remove(valutum);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Valuta {ime} sa šifrom {SifValuta} obrisana.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja valute: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji valuta sa šifrom: {SifValuta}";
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }


        private async Task PrepareDropDownLists()
        {
            var oznvaluta = await ctx.Valuta
                              .OrderBy(d => d.OznValute)
                              .Select(d => d.OznValute)
                              .ToListAsync();
         
           
            ViewBag.oznvaluta = new SelectList(oznvaluta, nameof(Valutum.OznValute));
        }

    }
}
