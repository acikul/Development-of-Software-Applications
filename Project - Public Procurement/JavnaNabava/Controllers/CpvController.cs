using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class CpvController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public CpvController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }















        // GET: Cpv
        public ActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Cpvs.AsNoTracking();
            int count = query.Count();

            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji CPV sifra.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Create));
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
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            var cpvi = query.Select(m => new CpvViewModel
            {
                SifCpv = m.SifCpv,
                OpisCpv = m.OpisCpv,

            })
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

            var model = new CpviViewModel
            {
                Cpvi = cpvi,
                PagingInfo = pagingInfo
            };
            return View(model);
        }


        // GET: Cpv/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }
        // POST: Cpv/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cpv cpv)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(cpv);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"CPV {cpv.OpisCpv} dodan. CPV vrijednost = {cpv.SifCpv}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(cpv);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(cpv);
            }
        }


















        // GET: Cpv/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id, int page = 1, int sort = 1, bool ascending = true)
        {
            var cpv = await ctx.Cpvs
                                  .AsNoTracking()
                                  .Where(m => m.SifCpv.Equals(id))
                                  .SingleOrDefaultAsync();
            if (cpv != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(cpv);
            }
            else
            {
                return NotFound($"Neispravna šifra CPV-a: {id}");
            }
        }

        // POST: Cpv/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cpv cpv, int page = 1, int sort = 1, bool ascending = true)
        {
            if (cpv == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Cpvs.AnyAsync(m => m.SifCpv.Equals(cpv.SifCpv));
            if (!checkId)
            {
                return NotFound($"Neispravna sifra cpv-a: {cpv?.SifCpv}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(cpv);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "CPV ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(cpv);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(cpv);
            }
        }
















        private async Task PrepareDropDownLists()
        {
        }
















        // POST: SluzbeniDokument/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string SifCpv, int page = 1, int sort = 1, bool ascending = true)
        {
            var cpv = ctx.Cpvs.Find(SifCpv);
            if (cpv != null)
            {
                try
                {
                    string naziv = cpv.OpisCpv;
                    ctx.Remove(cpv);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"CPV {naziv} uspješno obrisan";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja dokumenta: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji dokument sa šifrom: " + SifCpv;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
    }
}
