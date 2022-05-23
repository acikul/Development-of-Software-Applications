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
    public class PlanNabaveController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public PlanNabaveController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: PlanNabave
        public ActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.PlanNabaves.AsNoTracking();
            int count = query.Count();

            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedna planNabave na natječaj.";
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

            var planovi = query.Select(m => new PlanNabaveViewModel
            {
                SifPlanaNabave = m.SifPlanaNabave,
                VrijednostGodine = m.SifGodineNavigation.VrijednostGodine,
                OibNarucitelja = m.OibnaruciteljaNavigation.Oibnarucitelja,
                NazivNarucitelja = m.OibnaruciteljaNavigation.NazivNarucitelja,

            })
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();


            foreach(PlanNabaveViewModel plan in planovi)
            {
                plan.Stavke = ctx.StavkaPlanaNabaves
                                      .Where(s => s.SifPlanaNabave == plan.SifPlanaNabave)
                                      .OrderBy(s => s.SifStavke)
                                      .Select(s => new StavkaPlanaNabaveViewModel
                                      {
                                          SifStavke = s.SifStavke,
                                          EvidencijskiBroj = s.EvidencijskiBroj,
                                          PredmetNabave = s.PredmetNabave,
                                          VrijediOd = s.VrijediOd,
                                          VrijediDo = s.VrijediDo,
                                          Napomena = s.Napomena,
                                          VrstaPostupka = s.VrstaPostupka,
                                          VrijednostNabave = s.VrijednostNabave,
                                          NazivStatusa = s.SifStatusaNavigation.NazivStatusa,
                                     
                                          
                                      }).ToList();
            }

            var model = new PlanoviNabaveViewModel
            {
                PlanoviNabave = planovi,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        // GET: PlanoviNabave/Edit/5
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            await PrepareDropDownLists();
            var plan = await ctx.PlanNabaves
                                    .Where(m => m.SifPlanaNabave == id)
                                    .Select(m => new PlanNabaveViewModel
                                    {
                                        SifPlanaNabave = m.SifPlanaNabave,
                                        VrijednostGodine = m.SifGodineNavigation.VrijednostGodine,
                                        SifGodine = m.SifGodine,
                                        OibNarucitelja = m.Oibnarucitelja,
                                        
                                    })
                                    .FirstOrDefaultAsync();
            
            if (plan == null)
            {
                return NotFound($"plan {id} ne postoji");
            }
            else
            {
                
                var stavke = await ctx.StavkaPlanaNabaves
                                      .Where(s => s.SifPlanaNabave == plan.SifPlanaNabave)
                                      .OrderBy(s => s.SifStavke)
                                      .Select(s => new StavkaPlanaNabaveViewModel
                                      {
                                          EvidencijskiBroj = s.EvidencijskiBroj,
                                          SifStavke = s.SifStavke,
                                          PredmetNabave = s.PredmetNabave,
                                          VrijediOd = s.VrijediOd,
                                          VrijediDo = s.VrijediDo,
                                          Napomena = s.Napomena,
                                          VrstaPostupka = s.VrstaPostupka,
                                          VrijednostNabave = s.VrijednostNabave,
                                          SifStatusa = s.SifStatusa,
                                          NazivStatusa = s.SifStatusaNavigation.NazivStatusa,
                                         
                                      })
                                      .ToListAsync();
                plan.Stavke = stavke;

               

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
               


                return View(nameof(Edit), plan);
            }
        }

        // POST: PlanNabave/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlanNabaveViewModel planNabave, int page = 1, int sort = 1, bool ascending = true)
        {
            if (planNabave == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.PlanNabaves.AnyAsync(m => m.SifPlanaNabave == planNabave.SifPlanaNabave);
            if (!checkId)
            {
                return NotFound($"Neispravna sifra prijave: {planNabave?.SifPlanaNabave}");
            }

            if (ModelState.IsValid)
            {
                var planNabaveModel = await ctx.PlanNabaves
                                .Include(d => d.StavkaPlanaNabaves)
                                .Where(d => d.SifPlanaNabave == planNabave.SifPlanaNabave)
                                .FirstOrDefaultAsync();

                List<int> idStavki = planNabave.Stavke
                                 .Where(s => s.SifStavke > 0)
                                 .Select(s => s.SifStavke)
                                 .ToList();
                Console.WriteLine("ej imam ti listu ovih stvari");
                Console.WriteLine(idStavki);

                if(idStavki.Count == 0){
                    ctx.RemoveRange(planNabaveModel.StavkaPlanaNabaves.Where(s => true));

                } else {
                    ctx.RemoveRange(planNabaveModel.StavkaPlanaNabaves.Where(s => !idStavki.Contains(s.SifStavke)));

                }


                foreach (var stavkaPlana in planNabave.Stavke)
                {
                    StavkaPlanaNabave novaStavka;
                    Console.WriteLine("tavkaPlana.SifStavke > 0");
                    Console.WriteLine(stavkaPlana.SifStavke);
                    if(stavkaPlana.SifStavke > 0)
                    {
                        Console.Write("\n" + "stare stavke " + stavkaPlana.SifStavke);
                        novaStavka = planNabaveModel.StavkaPlanaNabaves.First(d => d.SifStavke == stavkaPlana.SifStavke);
                    }
                    else
                    {
                        
                        novaStavka = new StavkaPlanaNabave();
                        Console.Write("\n" + "nove stavke " + novaStavka.SifStavke);
                        planNabaveModel.StavkaPlanaNabaves.Add(novaStavka);
                    }
                    novaStavka.EvidencijskiBroj = stavkaPlana.EvidencijskiBroj;
                    novaStavka.PredmetNabave = stavkaPlana.PredmetNabave;
                    novaStavka.VrijediOd = stavkaPlana.VrijediOd;
                    novaStavka.VrijediDo = stavkaPlana.VrijediDo;
                    novaStavka.Napomena = stavkaPlana.Napomena;
                    novaStavka.VrstaPostupka = stavkaPlana.VrstaPostupka;
                    novaStavka.VrijednostNabave = stavkaPlana.VrijednostNabave;
                    novaStavka.SifStatusa = stavkaPlana.SifStatusa;
                    
                }
                try
                {

                   
                    ctx.Update(planNabaveModel);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "planNabave ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return View(planNabave);

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(planNabave);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(planNabave);
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
        public async Task<ActionResult> Create(PlanNabave planNabave)
        {
            
            if (ModelState.IsValid)
            {
                try
                {


                    ctx.Add(planNabave);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"planNabave dodan. Id plana nabave = {planNabave.SifPlanaNabave}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(planNabave);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(planNabave);
            }
        }



        // POST: PlanoviNabave/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int SifPlanaNabave, int page = 1, int sort = 1, bool ascending = true)
        {
            var planNabave = ctx.PlanNabaves.Find(SifPlanaNabave);
            if (planNabave != null)
            {
                try
                {
                    int sifra = planNabave.SifPlanaNabave;
                    ctx.Remove(planNabave);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"planNabave {sifra} uspješno obrisana";
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
                TempData[Constants.Message] = "Ne postoji planNabave sa šifrom: " + SifPlanaNabave;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }


        public async Task<IActionResult> Show(int id, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            var planNabave = await ctx.PlanNabaves
                                    .Where(m => m.SifPlanaNabave == id)
                                    .Select(m => new PlanNabaveViewModel
                                    {
                                        SifPlanaNabave = m.SifPlanaNabave,
                                        VrijednostGodine = m.SifGodineNavigation.VrijednostGodine,
                                        OibNarucitelja = m.OibnaruciteljaNavigation.Oibnarucitelja,
                                        NazivNarucitelja = m.OibnaruciteljaNavigation.NazivNarucitelja,

                                        
                                    })
                                    .FirstOrDefaultAsync();
            if (planNabave == null)
            {
                return NotFound($"planNabave {id} ne postoji");
            }
            else
            {
                //učitavanje stavki
               
                var stavkePlana = await ctx.StavkaPlanaNabaves
                                      .Where(s => s.SifPlanaNabave == planNabave.SifPlanaNabave)
                                      .OrderBy(s => s.SifStavke)
                                      .Select(s => new StavkaPlanaNabaveViewModel
                                      {
                                          SifStavke = s.SifStavke,
                                          EvidencijskiBroj = s.EvidencijskiBroj,
                                          PredmetNabave = s.PredmetNabave,
                                          VrijediOd = s.VrijediOd,
                                          VrijediDo = s.VrijediDo,
                                          Napomena = s.Napomena,
                                          VrstaPostupka = s.VrstaPostupka,
                                          VrijednostNabave = s.VrijednostNabave,
                                          NazivStatusa = s.SifStatusaNavigation.NazivStatusa
                                          
                                      }).ToListAsync();


                planNabave.Stavke = stavkePlana;

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                

                return View(viewName, planNabave);
            }
        }



        private async Task PrepareDropDownLists()
        {

            var godine = await ctx.Godinas
                                  .OrderBy(d => d.SifGodine)
                                  .Select(d => new {d.SifGodine, d.VrijednostGodine })
                                  .ToListAsync();

            var narucitelji = await ctx.Naruciteljs
                                  .OrderBy(d => d.Oibnarucitelja)
                                  .Select(d => new {d.Oibnarucitelja, d.NazivNarucitelja })
                                  .ToListAsync();

            


            ViewBag.godine = new SelectList(godine, nameof(Godina.SifGodine), nameof(Godina.VrijednostGodine));
            ViewBag.narucitelji = new SelectList(narucitelji, nameof(Narucitelj.Oibnarucitelja), nameof(Narucitelj.NazivNarucitelja));

        }
    }
}