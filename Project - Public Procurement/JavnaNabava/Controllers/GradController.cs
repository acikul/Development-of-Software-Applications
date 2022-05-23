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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JavnaNabava.Controllers
{


    public class GradController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public GradController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: Valuta
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Grads.AsNoTracking(); //or AsQueryable()
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

            var gradovi = await query.Select(a => new GradViewModel
            {
                SifGrada = a.SifGrada,
                NazivGrada = a.NazivGrada,
                SifDrzave = a.SifDrzava
            })
            .Skip((page - 1) * pagesize)
            .Take(pagesize)
            .ToListAsync();

            var model = new GradoviViewModel
            {
                Gradovi = gradovi,
                PagingInfo = pagingInfo
            };
            return View(model);

        }

        public IActionResult JTable()
        {

            return View(nameof(JTable));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grad = await ctx.Grads
                .FirstOrDefaultAsync(m => m.SifGrada == id);
            if (grad == null)
            {
                return NotFound();
            }

            return View(grad);
        }

        // GET: Grad/Create
        public IActionResult Create(int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            return View();
        }

        // POST: Grad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifGrada,NazivGrada,SifDrzave")] Grad grad, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(grad);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Grad {grad.NazivGrada} dodan. Sifra grada = {grad.SifGrada}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page = page, sort, ascending });
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Neuspjelo dodavanje grada {exc.CompleteExceptionMessage()}";
                    TempData[Constants.ErrorOccurred] = true;
                    return View(grad);
                }
            }
            TempData[Constants.Message] = $"Neuspjelo dodavanje grada";
            TempData[Constants.ErrorOccurred] = true;
            return View(grad);
        }

        // GET: Grad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var grad = await ctx.Grads
                                  .AsNoTracking()
                                  .Where(m => m.SifGrada == id)
                                  .SingleOrDefaultAsync();
            if (grad != null)
            {
                return PartialView(grad);
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji grad sa šifrom: {id}";
                TempData[Constants.ErrorOccurred] = true;
                return NotFound($"Neispravan id grada: {id}");
            }
        }


        // POST: Grad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grad grad)
        {
            if (grad == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Grads.AnyAsync(m => m.SifGrada == grad.SifGrada);
            if (!checkId)
            {
                return NotFound($"Neispravan id grada: {grad?.SifGrada}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(grad);
                    await ctx.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);

                    return PartialView(grad);
                }
            }
            else
            {

                return PartialView(grad);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var grad = await ctx.Grads
                                  .Where(m => m.SifGrada == id)
                                  .Select(a => new GradViewModel
                                  {
                                      SifGrada = a.SifGrada,
                                      SifDrzave = a.SifDrzava,
                                      NazivGrada = a.NazivGrada
                                  })
                                  .SingleOrDefaultAsync();
            if (grad != null)
            {
                return PartialView(grad);
            }
            else
            {
                return NotFound($"Neispravan id grada: {id}");
            }
        }

        // GET: Grad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grad = await ctx.Grads
                .FirstOrDefaultAsync(m => m.SifGrada == id);
            if (grad == null)
            {
                return NotFound();
            }

            return View(grad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var grad = await ctx.Grads.FindAsync(id);
            if (grad != null)
            {
                try
                {
                    string naziv = grad.NazivGrada;
                    ctx.Remove(grad);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Grad {naziv} sa šifrom {id} obrisana.",
                        successful = true
                    };

                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = "Pogreška prilikom brisanja grada: " + exc.CompleteExceptionMessage(),
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                var result = new
                {
                    message = $"Grad sa šifrom {id} ne postoji",
                    successful = false
                };
                return Json(result);

            }
        }

    }
}
