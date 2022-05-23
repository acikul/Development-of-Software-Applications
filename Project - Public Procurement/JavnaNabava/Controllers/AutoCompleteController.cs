using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    public class AutoCompleteController : Controller
    {
        private readonly RPPP23Context ctx;
        private readonly AppSettings appData;

        public AutoCompleteController(RPPP23Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public async Task<IEnumerable<IdStringLabel>> Ponuditelji(string term)
        {
            var query = ctx.Ponuditeljs
                            .Select(p => new IdStringLabel
                            {
                                Id = p.Oibponuditelja,
                                Label = p.NazivPonuditelja
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<IdLabel>> StrucneSpreme(string term)
        {
            var query = ctx.StrucnaSpremas
                            .Select(p => new IdLabel
                            {
                                Id = p.SifStrucneSpreme,
                                Label = p.NazivStrucneSpreme
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<IdLabel>> StrucneSpremeFiltered(string term)
        {
            var query = ctx.vw_Strucnjaci
                            .Select(p => new IdLabel
                            {
                                Id = p.SifStrucneSpreme,
                                Label = p.NazivStrucneSpreme
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();

            var listBezDuplica = new List<IdLabel>
            {
                list.First()
            };
            foreach (var d in list)
            {
                bool nema = true;
                foreach (var e in listBezDuplica)
                {
                    if (d.Label == e.Label)
                    {
                        nema = false;
                    }
                }
                if (nema)
                {
                    listBezDuplica.Add(d);
                }
            }

            return listBezDuplica;
        }

        public async Task<IEnumerable<IdLabel>> Gradovi(string term)
        {
            var query = ctx.Grads
                            .Select(p => new IdLabel
                            {
                                Id = p.SifGrada,
                                Label = p.NazivGrada
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();

            return list;
        }

        public async Task<IEnumerable<IdLabel>> GradoviFiltered(string term)
        {
            var query = ctx.vw_Strucnjaci
                            .Select(p => new IdLabel
                            {
                                Id = p.SifGrada,
                                Label = p.NazivGrada
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();

            var listBezDuplica = new List<IdLabel>
            {
                list.First()
            };
            foreach (var d in list)
            {
                bool nema = true;
                foreach (var e in listBezDuplica)
                {
                    if (d.Label == e.Label)
                    {
                        nema = false;
                    }
                }
                if (nema)
                {
                    listBezDuplica.Add(d);
                }
            }

            return listBezDuplica;
        }

        public async Task<IEnumerable<IdLabel>> VrsteKompetencija(string term)
        {
            var query = ctx.VrstaKompetencijes
                            .Select(p => new IdLabel
                            {
                                Id = p.SifVrsteKompetencije,
                                Label = p.NazivVrsteKompetencije
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<IdStringLabel>> Ponuditelj(string term)
        {
            var query = ctx.Ponuditeljs
                            .Where(a => a.NazivPonuditelja.Contains(term))
                            .Select(p => new IdStringLabel
                            {
                                Id = p.Oibponuditelja,
                                Label = p.NazivPonuditelja
                            });

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }
        public async Task<IEnumerable<IdStringLabel>> Konzorcij(string term)
        {
            var query = ctx.Konzorcijs
                            .Where(a => a.NazivKonzorcija.Contains(term))
                            .Select(p => new IdStringLabel
                            {
                                Id = p.Oibkonzorcija,
                                Label = p.NazivKonzorcija
                            });

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }
        public async Task<IEnumerable<IdLabel>> Natjecaj(string term)
        {
            var query = ctx.Natjecajs
                           .Select(p => new IdLabel
                           {
                               Id = p.SifNatjecaja,
                               Label = p.NazivNatjecaja
                           }).Where(l => l.Label.Contains(term)); ;

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }

        public async Task<IEnumerable<IdLabel>> VrstaDokumenta(string term)
        {
            var query = ctx.VrstaDokumenta
                           .Select(p => new IdLabel
                           {
                               Id = p.SifVrsteDokumenta,
                               Label = p.NazivVrsteDokumenta
                           })
                           .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }
    }
}
