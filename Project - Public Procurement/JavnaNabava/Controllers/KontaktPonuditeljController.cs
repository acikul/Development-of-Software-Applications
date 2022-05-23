using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.Extensions.Options;
using JavnaNabava.Extensions.Selectors;

namespace JavnaNabava.Controllers
{
    public class KontaktPonuditeljController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appData;

        public KontaktPonuditeljController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;

        }

        // GET: KontaktPonuditelj
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;
            var query = ctx.KontaktPonuditeljs.Include(k => k.OibponuditeljaNavigation).Include(k => k.SifVrsteKontaktaNavigation).AsNoTracking();
            int count = await query.CountAsync();

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

            var kontaktPonuditelji = await query.Skip((page - 1) * pagesize)
                          .Take(pagesize).ToListAsync();


            var model = new KontaktPonuditeljiViewModel
            {
                data = kontaktPonuditelji,
                PagingInfo = pagingInfo
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Oibponuditelja"] = new SelectList(ctx.Ponuditeljs, "Oibponuditelja", "NazivPonuditelja");
            ViewData["SifVrsteKontakta"] = new SelectList(ctx.VrstaKontakas, "SifVrsteKontakta", "NazivVrsteKontakta");
            return View();
        }

        // POST: KontaktPonuditelj/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifKontakt,TekstKontakta,Oibponuditelja,SifVrsteKontakta")] KontaktPonuditelj kontaktPonuditelj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(kontaktPonuditelj);
                    await ctx.SaveChangesAsync();
                    TempData["Message"] = "Kontakt uspješno dodan";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["Message"] = "Došlo je do pogreške prilikom spremanja kontakta";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            ViewData["Oibponuditelja"] = new SelectList(ctx.Ponuditeljs, "Oibponuditelja", "Oibponuditelja", kontaktPonuditelj.Oibponuditelja);
            ViewData["SifVrsteKontakta"] = new SelectList(ctx.VrstaKontakas, "SifVrsteKontakta", "NazivVrsteKontakta", kontaktPonuditelj.SifVrsteKontakta);
            return View(kontaktPonuditelj);
        }

        // GET: KontaktPonuditelj/Edit/5
        public async Task<IActionResult> Edit(int? id, int page = 1, int sort = 1, bool ascending = true)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kontaktPonuditelj = await ctx.KontaktPonuditeljs.Include(k => k.OibponuditeljaNavigation).Include(k => k.SifVrsteKontaktaNavigation).FirstOrDefaultAsync(i => i.SifKontakt == id);
            if (kontaktPonuditelj == null)
            {
                return NotFound();
            }
            ViewData["Oibponuditelja"] = new SelectList(ctx.Ponuditeljs, "Oibponuditelja", "NazivPonuditelja", kontaktPonuditelj.Oibponuditelja);
            ViewData["SifVrsteKontakta"] = new SelectList(ctx.VrstaKontakas, "SifVrsteKontakta", "NazivVrsteKontakta", kontaktPonuditelj.SifVrsteKontakta);
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            return View(kontaktPonuditelj);
        }

        // POST: KontaktPonuditelj/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifKontakt,TekstKontakta,Oibponuditelja,SifVrsteKontakta")] KontaktPonuditelj kontaktPonuditelj, int page = 1, int sort = 1, bool ascending = true)
        {
            if (id != kontaktPonuditelj.SifKontakt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(kontaktPonuditelj);
                    await ctx.SaveChangesAsync();
                    TempData["Message"] = "Promjene uspješno spremljene";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KontaktPonuditeljExists(kontaktPonuditelj.SifKontakt))
                    {
                        TempData["Message"] = "Došlo je do pogreške prilikom spremanja promjena";
                        TempData[Constants.ErrorOccurred] = true;
                        return NotFound();
                    }
                    else
                    {
                        TempData["Message"] = "Došlo je do pogreške prilikom spremanja promjena";
                        TempData[Constants.ErrorOccurred] = true;
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
            }
            else
            {
                kontaktPonuditelj = await ctx.KontaktPonuditeljs.Include(k => k.OibponuditeljaNavigation).Include(k => k.SifVrsteKontaktaNavigation).FirstOrDefaultAsync(i => i.SifKontakt == kontaktPonuditelj.SifKontakt);

            }

            ViewData["Oibponuditelja"] = new SelectList(ctx.Ponuditeljs, "Oibponuditelja", "Oibponuditelja", kontaktPonuditelj.Oibponuditelja);
            ViewData["SifVrsteKontakta"] = new SelectList(ctx.VrstaKontakas, "SifVrsteKontakta", "NazivVrsteKontakta", kontaktPonuditelj.SifVrsteKontakta);

            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;

            return View(kontaktPonuditelj);
        }

        // POST: KontaktPonuditelj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                var kontaktPonuditelj = await ctx.KontaktPonuditeljs.FindAsync(id);
                ctx.KontaktPonuditeljs.Remove(kontaktPonuditelj);
                await ctx.SaveChangesAsync();
                TempData["Message"] = "Kontakt uspješno obrisan";
                TempData[Constants.ErrorOccurred] = false;
            }
            catch (Exception)
            {
                TempData["Message"] = "Pogreška prlikom brisanja";
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        private bool KontaktPonuditeljExists(int id)
        {
            return ctx.KontaktPonuditeljs.Any(e => e.SifKontakt == id);
        }
    }
}
