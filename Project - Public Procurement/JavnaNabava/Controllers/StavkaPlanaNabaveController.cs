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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JavnaNabava.Controllers
{
    public class StavkaPlanaNabaveController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public StavkaPlanaNabaveController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: StavkaPlanaNabave
        public ActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.StavkaPlanaNabaves.AsNoTracking();
            int count = query.Count();

            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedna Stavka plana nabave na natječaj.";
                TempData[Constants.ErrorOccurred] = false;
                // return RedirectToAction(nameof(Create));
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

            var stavke = query.Select(m => new StavkaPlanaNabaveViewModel
            {
                SifStavke = m.SifStavke,
                EvidencijskiBroj = m.EvidencijskiBroj,
                PredmetNabave = m.PredmetNabave,
                Napomena = m.Napomena,
                VrijediOd = m.VrijediOd,
                VrijediDo = m.VrijediDo,
                VrstaPostupka = m.VrstaPostupka,
                VrijednostNabave = m.VrijednostNabave,
                SifStatusa = m.SifStatusa,
                NazivStatusa = m.SifStatusaNavigation.NazivStatusa,
                NazivNarucitelja = m.SifPlanaNabaveNavigation.OibnaruciteljaNavigation.NazivNarucitelja,


            })
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

            var model = new StavkePlanaNabaveViewModel
            {
                StavkePlanaNabave = stavke,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

      

       
        
    
    
    // GET: PlanoviNabave/Edit/5
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavkaPlanaNabave = await ctx.StavkaPlanaNabaves
                                  .AsNoTracking()
                                  .Where(m => m.SifStavke == id)
                                  .SingleOrDefaultAsync();
            if (stavkaPlanaNabave != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(stavkaPlanaNabave);
            }
            else
            {
                return NotFound($"Neispravna šifra prijave: {id}");
            }
        }

        // POST: stavkaPlanaNabave/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StavkaPlanaNabave stavkaPlanaNabave, int page = 1, int sort = 1, bool ascending = true)
        {
            if (stavkaPlanaNabave == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.StavkaPlanaNabaves.AnyAsync(m => m.SifStavke == stavkaPlanaNabave.SifStavke);
            if (!checkId)
            {
                return NotFound($"Neispravna sifra prijave: {stavkaPlanaNabave?.SifStavke}");
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if(stavkaPlanaNabave.Napomena == null){
                        stavkaPlanaNabave.Napomena = "";
                    }
                    ctx.Update(stavkaPlanaNabave);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "stavkaPlanaNabave ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(stavkaPlanaNabave);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(stavkaPlanaNabave);
            }
        }



        // GET: PlanNabave/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        // POST: PlanNabave/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StavkaPlanaNabave stavkaPlanaNabave)
        {
            
            
            if (ModelState.IsValid)
            {
                try
                {   
                    
                    if(stavkaPlanaNabave.Napomena == null){
                        stavkaPlanaNabave.Napomena = "";
                    }
                    ctx.Add(stavkaPlanaNabave);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"stavkaPlanaNabave dodan. Id plana nabave = {stavkaPlanaNabave.SifStavke}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {   
                    
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(stavkaPlanaNabave);
                }
            }
            else
            {
                Console.WriteLine("==========here");
                await PrepareDropDownLists();
                return View(stavkaPlanaNabave);
            }
        }



        // POST: PlanoviNabave/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int SifStavke, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavkaPlanaNabave = ctx.StavkaPlanaNabaves.Find(SifStavke);
            if (stavkaPlanaNabave != null)
            {
                try
                {
                    int sifra = stavkaPlanaNabave.SifPlanaNabave;
                    ctx.Remove(stavkaPlanaNabave);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"stavkaPlanaNabave {sifra} uspješno obrisana";
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
                TempData[Constants.Message] = "Ne postoji stavkaPlanaNabave sa šifrom: " + SifStavke;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }





        private async Task PrepareDropDownLists()
        {

            
            var narucitelji = await ctx.Naruciteljs
                                  .OrderBy(d => d.Oibnarucitelja)
                                  .Select(d => new {d.Oibnarucitelja, d.NazivNarucitelja })
                                  .ToListAsync();


            var planovi = await ctx.PlanNabaves
                                  .OrderBy(d => d.SifPlanaNabave)
                                  .Select(d => new {d.SifPlanaNabave, d.OibnaruciteljaNavigation.NazivNarucitelja })
                                  .ToListAsync();
            
            var statusi = await ctx.StatusStavkes
                                  .OrderBy(d => d.SifStatusa)
                                  .Select(d => new {d.SifStatusa, d.NazivStatusa })
                                  .ToListAsync();

                                  



            ViewBag.narucitelji = new SelectList(narucitelji, nameof(Narucitelj.Oibnarucitelja), nameof(Narucitelj.NazivNarucitelja));
            ViewBag.planovi = new SelectList(planovi, nameof(PlanNabave.SifPlanaNabave), nameof(PlanNabave.OibnaruciteljaNavigation.NazivNarucitelja));

            ViewBag.statusi = new SelectList(statusi, nameof(StatusStavke.SifStatusa), nameof(StatusStavke.NazivStatusa));


        }


    
    }

    
}