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
    public class GodinaController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;
        // GET: /<controller>/

        public GodinaController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: Valuta
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Godinas.AsNoTracking(); //or AsQueryable()
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


            var godine = await query.Select(a => new GodinaViewModel
            {
                SifGodine = a.SifGodine,
                VrijednostGodine = a.VrijednostGodine
            })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToListAsync();

            var model = new GodineViewModel
            {
                Godine = godine,
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

            var godina = await ctx.Godinas
                .FirstOrDefaultAsync(m => m.SifGodine == id);
            if (godina == null)
            {
                return NotFound();
            }

            return View(godina);
        }

        // GET: Godina/Create
        public IActionResult Create(int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            return View();
        }

        // POST: Godina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifGodina,VrijednostGodine")] Godina godina, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(godina);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Valuta {godina.VrijednostGodine} dodana. Sifra godine = {godina.SifGodine}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page = page, sort, ascending });
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Neuspjelo dodavanje godine {exc.CompleteExceptionMessage()}";
                    TempData[Constants.ErrorOccurred] = true;
                    return View(godina);
                }
            }
            TempData[Constants.Message] = $"Neuspjelo dodavanje godine";
            TempData[Constants.ErrorOccurred] = true;
            return View(godina);
        }

        // GET: Godina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var godina = await ctx.Godinas
                                  .AsNoTracking()
                                  .Where(m => m.SifGodine == id)
                                  .SingleOrDefaultAsync();
            if (godina != null)
            {
                return PartialView(godina);
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji godina sa šifrom: {id}";
                TempData[Constants.ErrorOccurred] = true;
                return NotFound($"Neispravan id godine: {id}");
            }
        }


        // POST: Valuta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Godina godina)
        {
            if (godina == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Godinas.AnyAsync(m => m.SifGodine == godina.SifGodine);
            if (!checkId)
            {
                return NotFound($"Neispravan id godine: {godina?.SifGodine}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(godina);
                    await ctx.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);

                    return PartialView(godina);
                }
            }
            else
            {

                return PartialView(godina);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var godina = await ctx.Godinas
                                  .Where(m => m.SifGodine == id)
                                  .Select(a => new GodinaViewModel
                                  {
                                      SifGodine = a.SifGodine,
                                      VrijednostGodine = a.VrijednostGodine

                                  })
                                  .SingleOrDefaultAsync();
            if (godina != null)
            {
                return PartialView(godina);
            }
            else
            {
                return NotFound($"Neispravan id godine: {id}");
            }
        }

        // GET: Godina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var godina = await ctx.Godinas
                .FirstOrDefaultAsync(m => m.SifGodine == id);
            if (godina == null)
            {
                return NotFound();
            }

            return View(godina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var godina = await ctx.Godinas.FindAsync(id);
            if (godina != null)
            {
                try
                {
                    int naziv = godina.VrijednostGodine;
                    ctx.Remove(godina);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Godina {naziv} sa šifrom {id} obrisana.",
                        successful = true
                    };

                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = "Pogreška prilikom brisanja godine: " + exc.CompleteExceptionMessage(),
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                var result = new
                {
                    message = $"Godina sa šifrom {id} ne postoji",
                    successful = false
                };
                return Json(result);

            }
        }
    }

}
