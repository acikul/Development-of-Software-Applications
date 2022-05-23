using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using Microsoft.AspNetCore.Mvc;
using JavnaNabava.Util.ExceptionFilters;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using JavnaNabava.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JavnaNabava.Controllers
{
    /// <summary>
    /// Web API servis za rad s valutama
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class VrstKompsController : ControllerBase, ICustomController<int, VrstaKompetencije>
    {
        private readonly RPPP23Context _context;
        private static Dictionary<string, Expression<Func<VrstaKompetencije, object>>> orderSelectors = new Dictionary<string, Expression<Func<VrstaKompetencije, object>>>
        {
            [nameof(VrstaKompetencije.SifVrsteKompetencije).ToLower()] = m => m.SifVrsteKompetencije,
            [nameof(VrstaKompetencije.NazivVrsteKompetencije).ToLower()] = m => m.NazivVrsteKompetencije

        };

        public VrstKompsController(RPPP23Context context)
        {
            _context = context;
        }


        [HttpGet("count", Name = "BrojVrstaKompetencija")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = _context.VrstaKompetencijes.AsQueryable();

            int count = await query.CountAsync();
            return count;
        }

        /// <summary>
        /// Dohvat svih vrsta kompetencija.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "DohvatiVrsteKompetencija")]
        public virtual async Task<List<VrstaKompetencije>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = _context.VrstaKompetencijes.AsQueryable();
            if (loadParams.SortColumn != null)
            {
                if (orderSelectors.TryGetValue(loadParams.SortColumn.ToLower(), out var expr))
                {
                    query = loadParams.Descending ? query.OrderByDescending(expr) : query.OrderBy(expr);
                }
            }

            var list = await query
                            .Skip(loadParams.StartIndex)
                            .Take(loadParams.Rows)
                            .ToListAsync();
            return list;
        }

        /// <summary>
        /// Dohvat odredene vrste kompetencije
        /// </summary>
        /// <param name="id">Sif vrste kompteencije koju trazimo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<VrstaKompetencije>> Get(int id)
        {
            var vrstKomp = await _context.VrstaKompetencijes.FindAsync(id);

            if (vrstKomp == null)
            {
                return NotFound();
            }

            return vrstKomp;
        }

        /// <summary>
        /// Kreiranje vrste kompetencije
        /// </summary>
        /// <param name="vrstKomp">Vrsta komptencije koju kreiramo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(VrstaKompetencije vrstKomp)
        {

            _context.VrstaKompetencijes.Add(vrstKomp);

            await _context.SaveChangesAsync();
            var addedVrstKomp = await Get(vrstKomp.SifVrsteKompetencije);

            return CreatedAtAction("GetVrstaKompetencije", new { id = vrstKomp.SifVrsteKompetencije }, addedVrstKomp);
        }


        /// <summary>
        /// Azuriranje vrste kompetencije
        /// </summary>
        /// <param name="id">Sif vrste kompetencije koju azurirmo</param>
        /// <param name="vrstKomp">Atributi vrste komptencije koju azuriramo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VrstaKompetencije vrstKomp)
        {
            if (id != vrstKomp.SifVrsteKompetencije)
            {
                return BadRequest();
            }

            _context.Entry(vrstKomp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VrstaKompetencijeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // DELETE: api/VrsteKompetencija/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vrstKomp = await _context.VrstaKompetencijes.FindAsync(id);
            if (vrstKomp == null)
            {
                return NotFound();
            }

            _context.VrstaKompetencijes.Remove(vrstKomp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VrstaKompetencijeExists(int id)
        {
            return _context.VrstaKompetencijes.Any(e => e.SifVrsteKompetencije == id);
        }
    }
}
