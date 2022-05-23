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
    public class DrzavaController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;
        // GET: /<controller>/

        public DrzavaController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: Valuta
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Drzavas.AsNoTracking(); //or AsQueryable()
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

            var drzave = await query.Select(a => new DrzavaViewModel
            {
                SifDrzava = a.SifDrzave,
                NazivDrzava = a.NazivDrzava
            })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToListAsync();

            var model = new DrzaveViewModel
            {
                Drzave = drzave,
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

            var drzava = await ctx.Drzavas
                .FirstOrDefaultAsync(m => m.SifDrzave == id);
            if (drzava == null)
            {
                return NotFound();
            }

            return View(drzava);
        }

        // GET: Drzava/Create
        public IActionResult Create(int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            return View();
        }

        // POST: Drzava/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifDrzava,NazivDrzava")] Drzava drzava, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(drzava);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Valuta {drzava.NazivDrzava} dodana. Sifra drzave = {drzava.SifDrzave}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page = page, sort, ascending });
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Neuspjelo dodavanje drzave {exc.CompleteExceptionMessage()}";
                    TempData[Constants.ErrorOccurred] = true;
                    return View(drzava);
                }
            }
            TempData[Constants.Message] = $"Neuspjelo dodavanje drzave";
            TempData[Constants.ErrorOccurred] = true;
            return View(drzava);
        }

        // GET: Drzava/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var drzava = await ctx.Drzavas
                                  .AsNoTracking()
                                  .Where(m => m.SifDrzave == id)
                                  .SingleOrDefaultAsync();
            if (drzava != null)
            {
                return PartialView(drzava);
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji drzava sa šifrom: {id}";
                TempData[Constants.ErrorOccurred] = true;
                return NotFound($"Neispravan id drzave: {id}");
            }
        }


        // POST: Valuta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Drzava drzava)
        {
            if (drzava == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Drzavas.AnyAsync(m => m.SifDrzave == drzava.SifDrzave);
            if (!checkId)
            {
                return NotFound($"Neispravan id drzave: {drzava?.SifDrzave}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(drzava);
                    await ctx.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);

                    return PartialView(drzava);
                }
            }
            else
            {

                return PartialView(drzava);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var drzava = await ctx.Drzavas
                                  .Where(m => m.SifDrzave == id)
                                  .Select(a => new DrzavaViewModel
                                  {
                                      SifDrzava = a.SifDrzave,
                                      NazivDrzava = a.NazivDrzava

                                  })
                                  .SingleOrDefaultAsync();
            if (drzava != null)
            {
                return PartialView(drzava);
            }
            else
            {
                return NotFound($"Neispravan id drzave: {id}");
            }
        }

        // GET: Drzava/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drzava = await ctx.Drzavas
                .FirstOrDefaultAsync(m => m.SifDrzave == id);
            if (drzava == null)
            {
                return NotFound();
            }

            return View(drzava);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var drzava = await ctx.Drzavas.FindAsync(id);
            if (drzava != null)
            {
                try
                {
                    string naziv = drzava.NazivDrzava;
                    ctx.Remove(drzava);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Drzava {naziv} sa šifrom {id} obrisana.",
                        successful = true
                    };

                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = "Pogreška prilikom brisanja drzave: " + exc.CompleteExceptionMessage(),
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                var result = new
                {
                    message = $"Drzava sa šifrom {id} ne postoji",
                    successful = false
                };
                return Json(result);

            }
        }
    }

}
