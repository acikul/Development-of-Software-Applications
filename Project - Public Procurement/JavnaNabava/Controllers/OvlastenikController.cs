using JavnaNabava.Extensions;
using JavnaNabava.Extensions.Selectors;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    public class OvlastenikController : Controller
    {

        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public OvlastenikController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }



        // GET: OvlastenikController
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appSettings.PageSize;
            var query = ctx.Ovlasteniks.AsNoTracking(); //or AsQueryable()
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

            var ovlastenici = await query.Select(a => new OvlastenikViewModel
            {
                Oibovlastenika = a.Oibovlastenika,
                ImeOvlastenika  = a.ImeOvlastenika,
                PrezimeOvlastenika  = a.PrezimeOvlastenika,
                FunkcijaOvlastenika = a.FunkcijaOvlastenika,
                Oibnarucitelj = a.Oibnarucitelj
            })
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToListAsync();

            var model = new OvlasteniciViewModel
            {
                Ovlastenici = ovlastenici,
                PagingInfo = pagingInfo
            };
            return View(model);


        }


        public async Task<IActionResult> JTable()
        {

            return View(nameof(JTable));

        }



        // GET: OvlastenikController/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ovlastenik = await ctx.Ovlasteniks
                .FirstOrDefaultAsync(m => m.Oibovlastenika == id);
            if (ovlastenik == null)
            {
                return NotFound();
            }

            return View(ovlastenik);
        }

        // GET: OvlastenikController/Create
        public IActionResult Create(int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            return View();
        }

        // POST: OvlastenikController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Oibovlastenika,ImeOvlastenika,PrezimeOvlastenika,FunkcijaOvlastenika,Oibnarucitelj")] Ovlastenik ovlastenik, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewData["page"] = page;
            ViewData["sort"] = sort;
            ViewData["ascending"] = ascending;
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(ovlastenik);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Ovlastenik {ovlastenik.ImeOvlastenika} dodan. Oib ovlastenika = {ovlastenik.Oibovlastenika}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page = page, sort, ascending });
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Neuspjelo dodavanje ovlastenika {exc.CompleteExceptionMessage()}";
                    TempData[Constants.ErrorOccurred] = true;
                    return View(ovlastenik);
                }
            }
            TempData[Constants.Message] = $"Neuspjelo dodavanje ovlastenika";
            TempData[Constants.ErrorOccurred] = true;
            return View(ovlastenik);
        }

        // GET: OvlastenikController/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            var ovlastenik = await ctx.Ovlasteniks
                                  .AsNoTracking()
                                  .Where(m => m.Oibovlastenika == id)
                                  .SingleOrDefaultAsync();
            if (ovlastenik != null)
            {
                return PartialView(ovlastenik);
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji ovlastenik sa oibom: {id}";
                TempData[Constants.ErrorOccurred] = true;
                return NotFound($"Neispravan oib ovlastenika: {id}");
            }
        }

        // POST: OvlastenikController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Ovlastenik ovlastenik)
        {
            if (ovlastenik == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Ovlasteniks.AnyAsync(m => m.Oibovlastenika == ovlastenik.Oibovlastenika);
            if (!checkId)
            {
                return NotFound($"Neispravan oib ovlastenika: {ovlastenik?.Oibovlastenika}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(ovlastenik);
                    await ctx.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);

                    return PartialView(ovlastenik);
                }
            }
            else
            {

                return PartialView(ovlastenik);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var ovlastenik = await ctx.Ovlasteniks
                                  .Where(m => m.Oibovlastenika == id)
                                  .Select(a => new OvlastenikViewModel
                                  {
                                      Oibovlastenika = a.Oibovlastenika,
                                      ImeOvlastenika = a.ImeOvlastenika,
                                      PrezimeOvlastenika = a.PrezimeOvlastenika,
                                      FunkcijaOvlastenika = a.FunkcijaOvlastenika,
                                      Oibnarucitelj = a.Oibnarucitelj

                                  })
                                  .SingleOrDefaultAsync();
            if (ovlastenik != null)
            {
                return PartialView(ovlastenik);
            }
            else
            {
                return NotFound($"Neispravan oib ovlastenika: {id}");
            }
        }


        // GET: OvlastenikController/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ovlastenik = await ctx.Ovlasteniks
                .FirstOrDefaultAsync(m => m.Oibovlastenika == id);
            if (ovlastenik == null)
            {
                return NotFound();
            }

            return View(ovlastenik);
        }

        // POST: OvlastenikController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletepost(string oib)

        {
            var ovlastenik = await ctx.Ovlasteniks.FindAsync(oib);
            if (ovlastenik != null)
            {
                try
                {
                    string naziv = ovlastenik.ImeOvlastenika;
                    ctx.Remove(ovlastenik);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Ovlastenik {naziv} sa oibom {oib} obrisan.",
                        successful = true
                    };

                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = "Pogreška prilikom brisanja ovlastenika: " + exc.CompleteExceptionMessage(),
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                var result = new
                {
                    message = $"Ovlastenik sa oibom {oib} ne postoji",
                    successful = false
                };
                return Json(result);

            }
        }




    }
}
