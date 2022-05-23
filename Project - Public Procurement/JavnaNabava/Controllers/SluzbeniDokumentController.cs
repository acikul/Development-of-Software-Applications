using System;
using System.Collections.Generic;
using System.IO;
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
    public class SluzbeniDokumentController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appSettings;

        public SluzbeniDokumentController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appSettings = options.Value;
        }

        // GET: SluzbeniDokument
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.SluzbeniDokuments.AsNoTracking();
            int count = await query.CountAsync();

            if (count == 0)
            {
                TempData[Constants.Message] = "Ne postoji niti jedan dokument.";
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

            var sluzbeniDokumenti = await query.Select(m => new SluzbeniDokumentViewModel
            {
                SifDokumenta = m.SifDokumenta,
                KlasaDokumenta = m.KlasaDokumenta,
                ImeDokumenta = m.ImeDokumenta,
                UrudzbeniBroj = m.UrudzbeniBroj,
                TekstDokumenta = m.TekstDokumenta,
                VrstaDokumenta = m.SifVrsteDokumentaNavigation.NazivVrsteDokumenta,
                SifPrijave = m.SifPrijave
            })
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToListAsync();

            var model = new SluzbeniDokumentiViewModel
            {
                SluzbeniDokumenti = sluzbeniDokumenti,
                PagingInfo = pagingInfo
            };
            return View(model);
        }



        // GET: SluzbeniDokument/Create
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        // POST: SluzbeniDokument/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SluzbeniDokument dokument, IFormFile binarniZapis)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (binarniZapis != null && binarniZapis.Length > 0)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            binarniZapis.CopyTo(stream);
                            byte[] tekst = stream.ToArray();
                            dokument.TekstDokumenta = tekst;
                        }
                    }
                    ctx.Add(dokument);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Dokument {dokument.ImeDokumenta} dodan. Id dokumenta = {dokument.SifDokumenta}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(dokument);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(dokument);
            }
        }

        public async Task<FileContentResult> GetFile(int id)
        {
            byte[] binarniZapis = await ctx.SluzbeniDokuments
                                    .Where(a => a.SifDokumenta == id)
                                    .Select(a => a.TekstDokumenta)
                                    .SingleOrDefaultAsync();

            if (binarniZapis != null)
            {
                return File(binarniZapis, "file/txt");
            }
            else
            {
                return null;
            }
        }

        // GET: SluzbeniDokument/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var dokument = await ctx.SluzbeniDokuments
                                  .AsNoTracking()
                                  .Where(m => m.SifDokumenta == id)
                                  .SingleOrDefaultAsync();
            if (dokument != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(dokument);
            }
            else
            {
                return NotFound($"Neispravna šifra dokumenta: {id}");
            }
        }

        // POST: SluzbeniDokument/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SluzbeniDokument dokument, IFormFile binarniZapis, int page = 1, int sort = 1, bool ascending = true)
        {
            if (dokument == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            SluzbeniDokument novi = await ctx.SluzbeniDokuments.FindAsync(dokument.SifDokumenta);
            if (novi==null)
            {
                return NotFound($"Neispravna sifra dokumenta: {dokument?.SifDokumenta}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    novi.SifDokumenta = dokument.SifDokumenta;
                    novi.ImeDokumenta = dokument.ImeDokumenta;
                    novi.KlasaDokumenta = dokument.KlasaDokumenta;
                    novi.UrudzbeniBroj = dokument.UrudzbeniBroj;
                    novi.SifVrsteDokumenta = dokument.SifVrsteDokumenta;
                    novi.SifPrijave = dokument.SifPrijave;
                    if (binarniZapis != null && binarniZapis.Length > 0)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            binarniZapis.CopyTo(stream);
                            byte[] tekst = stream.ToArray();
                            novi.TekstDokumenta = tekst;
                        }
                    }
                    
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Dokument ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page , sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return PartialView(dokument);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return PartialView(dokument);
            }
        }

        private async Task PrepareDropDownLists()
        {

            var vrsteDokumenta = await ctx.VrstaDokumenta
                                  .OrderBy(d => d.SifVrsteDokumenta)
                                  .Select(d => new { d.NazivVrsteDokumenta, d.SifVrsteDokumenta })
                                  .ToListAsync();
            var prijave = await ctx.PrijavaNaNatjecajs
                                 .OrderBy(d => d.SifPrijava)
                                 .Select(d => d.SifPrijava)
                                 .ToListAsync();

            ViewBag.vrsteDokumenta = new SelectList(vrsteDokumenta, nameof(VrstaDokumentum.SifVrsteDokumenta), nameof(VrstaDokumentum.NazivVrsteDokumenta));
            ViewBag.prijave = new SelectList(prijave, nameof(PrijavaNaNatjecaj.SifPrijava));
        }

        // POST: SluzbeniDokument/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int SifDokumenta, int page = 1, int sort = 1, bool ascending = true)
        {
            var dokument = ctx.SluzbeniDokuments.Find(SifDokumenta);
            if (dokument != null)
            {
                try
                {
                    string naziv = dokument.ImeDokumenta;
                    ctx.Remove(dokument);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Dokument {naziv} uspješno obrisan";
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
                TempData[Constants.Message] = "Ne postoji dokument sa šifrom: " + SifDokumenta;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }


    }   
}