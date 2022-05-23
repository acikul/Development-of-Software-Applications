using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.Controllers
{
    public class PonuditeljController : Controller
    {
        private readonly RPPP23Context ctx;

        public PonuditeljController(RPPP23Context context)
        {
            ctx = context;
        }

        // GET: Ponuditelj
        public async Task<IActionResult> Index()
        {
            var rPPP23Context = ctx.Ponuditeljs.Include(p => p.OibkonzorcijaNavigation).Include(p => p.SifGradaNavigation).Include(p => p.KontaktPonuditeljs);
            return View(await rPPP23Context.ToListAsync());
        }

        // GET: Ponuditelj/Details/5
        public async Task<IActionResult> Details(string id, string exController, int page = 1, int sort = 1, bool ascending = true)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ponuditelj = await ctx.Ponuditeljs
                .Include(p => p.OibkonzorcijaNavigation)
                .Include(p => p.SifGradaNavigation)
                .Include(p => p.KontaktPonuditeljs)
                .FirstOrDefaultAsync(m => m.Oibponuditelja == id);
            if (ponuditelj == null)
            {
                return NotFound();
            }

            var kontakti = await ctx.KontaktPonuditeljs.Include(k => k.SifVrsteKontaktaNavigation).Where(k => k.Oibponuditelja == id).ToListAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending
            };

            var model = new PonuditeljViewModel
            {
                ponuditelj = ponuditelj,
                kontakti = kontakti,
                prethodniKontroler = exController,
                PagingInfo = pagingInfo
            };

            return View(model);
        }


        private bool PonuditeljExists(string id)
        {
            return ctx.Ponuditeljs.Any(e => e.Oibponuditelja == id);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string exController, int page = 1, int sort = 1, bool ascending = true)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await ctx.Ponuditeljs
                .Include(p => p.SifGradaNavigation)
                .Select(ponuditelj => new Ponuditelj2ViewModel
                {
                    AdresaPonuditelja = ponuditelj.AdresaPonuditelja,
                    KlasaPonuditelja = ponuditelj.KlasaPonuditelja,
                    NazivGrada = ponuditelj.SifGradaNavigation.NazivGrada,
                    NazivPonuditelja = ponuditelj.NazivPonuditelja,
                    Oibkonzorcija = ponuditelj.Oibkonzorcija,
                    Oibponuditelja = ponuditelj.Oibponuditelja,
                    PbrPonuditelja = ponuditelj.PbrPonuditelja,
                    SifGrada = ponuditelj.SifGrada
                })
                .FirstOrDefaultAsync(m => m.Oibponuditelja == id);
            if (model == null)
            {
                return NotFound();
            }

            var kontakti = await ctx.KontaktPonuditeljs.Include(k => k.SifVrsteKontaktaNavigation).Where(k => k.Oibponuditelja == id)
                .Select(k => new KontaktViewModel
                {
                    TekstKontakta = k.TekstKontakta,
                    SifKontakt = k.SifKontakt,
                    SifVrsteKontakta = k.SifVrsteKontakta,
                    VrstaKontakta = k.SifVrsteKontaktaNavigation.NazivVrsteKontakta
                })
                .ToListAsync();

            ViewData["oibKonzorcija"] = new SelectList(ctx.Konzorcijs, "Oibkonzorcija", "NazivKonzorcija", model.Oibkonzorcija);


            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending
            };

            model.PagingInfo = pagingInfo;
            model.kontakti = kontakti;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ponuditelj2ViewModel model, string exController, int page = 1, int sort = 1, bool ascending = true)
        {
            Console.WriteLine(model.ToString());
            
            if (ModelState.IsValid)
            {
                Console.WriteLine("2");
                model.PagingInfo = new PagingInfo
                {
                    Ascending = ascending,
                    CurrentPage = page,
                    Sort = sort
                };
                var dokument = await ctx.Ponuditeljs
                                         .Include(p => p.OibkonzorcijaNavigation)
                                         .Include(p => p.SifGradaNavigation)
                                         .Include(p => p.KontaktPonuditeljs)
                                         .FirstOrDefaultAsync(m => m.Oibponuditelja == model.Oibponuditelja);
                if (dokument == null)
                {
                    Console.WriteLine("3");
                    return NotFound("Ne postoji dokument s id-om: " + model.Oibponuditelja);
                }


                dokument.NazivPonuditelja = model.NazivPonuditelja;
                dokument.KlasaPonuditelja = model.KlasaPonuditelja;
                dokument.AdresaPonuditelja = model.AdresaPonuditelja;
                dokument.Oibkonzorcija = model.Oibkonzorcija;
                dokument.PbrPonuditelja = model.PbrPonuditelja;
                dokument.SifGrada = model.SifGrada;
                dokument.Oibponuditelja = model.Oibponuditelja;


                List<int> idStavki = model.kontakti
                                          .Where(s => s.SifVrsteKontakta > 0)
                                          .Select(s => s.SifKontakt)
                                          .ToList();
                //izbaci sve koje su nisu više u modelu
                ctx.RemoveRange(dokument.KontaktPonuditeljs.Where(s => !idStavki.Contains(s.SifKontakt)));

                foreach (var kontakt in model.kontakti)
                {
                    //ažuriraj postojeće i dodaj nove
                    KontaktPonuditelj noviKontakt; // potpuno nova ili dohvaćena ona koju treba izmijeniti
                    if (kontakt.SifKontakt > 0)
                    {
                        noviKontakt = dokument.KontaktPonuditeljs.First(s => s.SifKontakt == kontakt.SifKontakt);
                    }
                    else
                    {
                        noviKontakt = new KontaktPonuditelj();
                        dokument.KontaktPonuditeljs.Add(noviKontakt);
                    }
                    noviKontakt.TekstKontakta = kontakt.TekstKontakta;
                    noviKontakt.SifVrsteKontakta = kontakt.SifVrsteKontakta;
                    noviKontakt.Oibponuditelja = model.Oibponuditelja;
                }

                try
                {
                    Console.WriteLine("4");

                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Ponuditelj {dokument.NazivPonuditelja} uspješno ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = dokument.Oibponuditelja,
                        exController,
                        page,
                        sort,
                        ascending
                    });

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    Console.WriteLine("5");
                    return View(model);
                }
            }
            else
            {
                Console.WriteLine("6");
                model.PagingInfo = new PagingInfo
                {
                    Ascending = ascending,
                    CurrentPage = page,
                    Sort = sort
                };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string filter, int page = 1, int sort = 1, bool ascending = true)
        {
            var ponuditelj = await ctx.Ponuditeljs
                                    .Where(d => d.Oibponuditelja == id)
                                    .SingleOrDefaultAsync();

            var kontakti = await ctx.KontaktPonuditeljs.Where(k => k.Oibponuditelja == id).ToListAsync();
            var prijave = await ctx.PrijavaNaNatjecajs.Where(k => k.Oibponuditelja == id).ToListAsync();
            var strucnjaci = await ctx.Strucnjaks.Where(k => k.Oibponuditelja == id).ToListAsync();


            if (ponuditelj != null)
            {
                try
                {
                    ctx.RemoveRange(kontakti);
                    ctx.RemoveRange(prijave);
                    ctx.RemoveRange(strucnjaci);

                    ctx.Remove(ponuditelj);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Dokument {ponuditelj.Oibponuditelja} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja dokumenta: " + exc.Message;
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = "Ne postoji ponuditelj s oibom-om: " + id;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var dokument = new Ponuditelj2ViewModel();
            ViewData["oibKonzorcija"] = new SelectList(ctx.Konzorcijs, "Oibkonzorcija", "NazivKonzorcija", dokument.Oibkonzorcija);
            return View(dokument);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ponuditelj2ViewModel model)
        {
            if (ModelState.IsValid)
            {
                Ponuditelj p = new Ponuditelj();
                p.Oibponuditelja = model.Oibponuditelja;
                p.NazivPonuditelja = model.NazivPonuditelja;
                p.Oibkonzorcija = model.Oibkonzorcija;
                p.KlasaPonuditelja = model.KlasaPonuditelja;
                p.PbrPonuditelja = model.PbrPonuditelja;
                p.SifGrada = model.SifGrada;
                p.AdresaPonuditelja = model.AdresaPonuditelja;

                foreach (var k in model.kontakti)
                {
                    KontaktPonuditelj k1 = new KontaktPonuditelj();
                    k1.SifVrsteKontakta = k.SifVrsteKontakta;
                    k1.TekstKontakta = k.TekstKontakta;
                    k1.Oibponuditelja = model.Oibponuditelja;
                    p.KontaktPonuditeljs.Add(k1);
                }
                
                try
                {
                    ctx.Add(p);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Ponuditlej uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
