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
    /// Web API servis za rad s kriterijima
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class KriterijiController : ControllerBase, ICustomController<int, Kriterij>
    {
        private readonly RPPP23Context _context;
        private static Dictionary<string, Expression<Func<Kriterij, object>>> orderSelectors = new Dictionary<string, Expression<Func<Kriterij, object>>>
        {
            [nameof(Kriterij.SifKriterija).ToLower()] = m => m.SifKriterija,
            [nameof(Kriterij.NazivKriterija).ToLower()] = m => m.NazivKriterija

        };

        public KriterijiController(RPPP23Context context)
        {
            _context = context;
        }


        [HttpGet("count", Name = "BrojKriterija")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = _context.Kriterijs.AsQueryable();

            int count = await query.CountAsync();
            return count;
        }

        /// <summary>
        /// Dohvat svih Kriterija.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "DohvatiKriterije")]
        public virtual async Task<List<Kriterij>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = _context.Kriterijs.AsQueryable();
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
        /// Dohvat odredenog Kriterija
        /// </summary>
        /// <param name="id">Id Kriterija kojeg trazimo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Kriterij>> Get(int id)
        {
            var kriterij = await _context.Kriterijs.FindAsync(id);

            if (kriterij == null)
            {
                return NotFound();
            }

            return kriterij;
        }

        /// <summary>
        /// Kreiranje Kriterija
        /// </summary>
        /// <param name="Kriterij">Kriterij koju kreiramo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Kriterij kriterij)
        {

            _context.Kriterijs.Add(kriterij);

            await _context.SaveChangesAsync();
            var addedKriterij = await Get(kriterij.SifKriterija);

            return CreatedAtAction("GetKriterij", new { id = kriterij.SifKriterija }, addedKriterij);
        }


        /// <summary>
        /// Azuriranje Kriterija
        /// </summary>
        /// <param name="id">Id Kriterija kojeg azurirmo</param>
        /// <param name="Kriterij">Atributi Kriterija kojeg azuriramo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Kriterij kriterij)
        {
            if (id != kriterij.SifKriterija)
            {
                return BadRequest();
            }

            _context.Entry(kriterij).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KriterijExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kriterij = await _context.Kriterijs.FindAsync(id);
            if (kriterij == null)
            {
                return NotFound();
            }

            _context.Kriterijs.Remove(kriterij);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KriterijExists(int id)
        {
            return _context.Kriterijs.Any(e => e.SifKriterija == id);
        }
    }
}
