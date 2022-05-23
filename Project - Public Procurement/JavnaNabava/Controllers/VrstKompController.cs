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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JavnaNabava.Controllers
{
    public class VrstKompController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;
        
        // GET: /<controller>/
        public VrstKompController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: VrstaKompetencije
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.VrstaKompetencijes.AsNoTracking(); //or AsQueryable()
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

            var vrstKomps = await query.Select(a => new VrstKompViewModel
            {
                SifVrsteKompetencije = a.SifVrsteKompetencije,
                NazivVrsteKompetencije = a.NazivVrsteKompetencije
            })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToListAsync();

            var model = new VrstKompsViewModel
            {
                VrsteKompetencija = vrstKomps,
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

            var vrstKomp = await ctx.VrstaKompetencijes
                .FirstOrDefaultAsync(m => m.SifVrsteKompetencije == id);
            if (vrstKomp == null)
            {
                return NotFound();
            }

            return View(vrstKomp);
        }

        // GET: VrstaKompetencije/Create
        public IActionResult Create(int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            return View();
        }

        // POST: VrstaKompetencije/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifDrzava,NazivDrzava")] VrstaKompetencije vrstKomp, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(vrstKomp);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Vrsta kompetencije {vrstKomp.NazivVrsteKompetencije} dodana. Sifra = {vrstKomp.SifVrsteKompetencije}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page = page, sort, ascending });
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Neuspjelo dodavanje drzave {exc.CompleteExceptionMessage()}";
                    TempData[Constants.ErrorOccurred] = true;
                    return View(vrstKomp);
                }
            }
            TempData[Constants.Message] = $"Neuspjelo dodavanje drzave";
            TempData[Constants.ErrorOccurred] = true;
            return View(vrstKomp);
        }

        // GET: VrstaKompetencije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var vrstKomp = await ctx.VrstaKompetencijes
                                  .AsNoTracking()
                                  .Where(m => m.SifVrsteKompetencije == id)
                                  .SingleOrDefaultAsync();
            if (vrstKomp != null)
            {
                return PartialView(vrstKomp);
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji vrsta kompetencije sa šifrom: {id}";
                TempData[Constants.ErrorOccurred] = true;
                return NotFound($"Neispravna sifra vrste kompetencije: {id}");
            }
        }


        // POST: VrstaKompetencije/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VrstaKompetencije vrstKomp)
        {
            if (vrstKomp == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.VrstaKompetencijes.AnyAsync(m => m.SifVrsteKompetencije == vrstKomp.SifVrsteKompetencije);
            if (!checkId)
            {
                return NotFound($"Neispravan id vrste kompetencije: {vrstKomp?.SifVrsteKompetencije}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(vrstKomp);
                    await ctx.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);

                    return PartialView(vrstKomp);
                }
            }
            else
            {

                return PartialView(vrstKomp);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var vrstKomp = await ctx.VrstaKompetencijes
                                  .Where(m => m.SifVrsteKompetencije == id)
                                  .Select(a => new VrstKompViewModel
                                  {
                                      SifVrsteKompetencije = a.SifVrsteKompetencije,
                                      NazivVrsteKompetencije = a.NazivVrsteKompetencije

                                  })
                                  .SingleOrDefaultAsync();
            if (vrstKomp != null)
            {
                return PartialView(vrstKomp);
            }
            else
            {
                return NotFound($"Neispravna šifra vrste drzave: {id}");
            }
        }

        // GET: VrstaKompetencije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrstKomp = await ctx.VrstaKompetencijes
                .FirstOrDefaultAsync(m => m.SifVrsteKompetencije == id);
            if (vrstKomp == null)
            {
                return NotFound();
            }

            return View(vrstKomp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var vrstKomp = await ctx.VrstaKompetencijes.FindAsync(id);
            if (vrstKomp != null)
            {
                try
                {
                    string naziv = vrstKomp.NazivVrsteKompetencije;
                    ctx.Remove(vrstKomp);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Vrsta kompetencije {naziv} sa šifrom {id} obrisana.",
                        successful = true
                    };

                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = "Pogreška prilikom brisanja vrste kompetencije: " + exc.CompleteExceptionMessage(),
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                var result = new
                {
                    message = $"Vrsta kompetencije sa šifrom {id} ne postoji",
                    successful = false
                };
                return Json(result);

            }
        }
    }

}
