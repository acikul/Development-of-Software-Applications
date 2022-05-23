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
    public class DrzaveController : ControllerBase, ICustomController<int, Drzava>
    {
        private readonly RPPP23Context _context;
        private static Dictionary<string, Expression<Func<Drzava, object>>> orderSelectors = new Dictionary<string, Expression<Func<Drzava, object>>>
        {
            [nameof(Drzava.SifDrzave).ToLower()] = m => m.SifDrzave,
            [nameof(Drzava.NazivDrzava).ToLower()] = m => m.NazivDrzava

        };

        public DrzaveController(RPPP23Context context)
        {
            _context = context;
        }


        [HttpGet("count", Name = "BrojDrzava")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = _context.Drzavas.AsQueryable();

            int count = await query.CountAsync();
            return count;
        }

        /// <summary>
        /// Dohvat svih drzava.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "DohvatiDrzave")]
        public virtual async Task<List<Drzava>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = _context.Drzavas.AsQueryable();
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
        /// Dohvat odredene drzave
        /// </summary>
        /// <param name="id">Id drzave koju trazimo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Drzava>> Get(int id)
        {
            var drzava = await _context.Drzavas.FindAsync(id);

            if (drzava == null)
            {
                return NotFound();
            }

            return drzava;
        }

        /// <summary>
        /// Kreiranje drzave
        /// </summary>
        /// <param name="drzava">Drzava koju kreiramo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Drzava drzava)
        {

            _context.Drzavas.Add(drzava);

            await _context.SaveChangesAsync();
            var addedDrzava = await Get(drzava.SifDrzave);

            return CreatedAtAction("GetDrzava", new { id = drzava.SifDrzave }, addedDrzava);
        }


        /// <summary>
        /// Azuriranje drzave
        /// </summary>
        /// <param name="id">Id drzave koju azurirmo</param>
        /// <param name="drzava">Atributi drzave koju azuriramo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Drzava drzava)
        {
            if (id != drzava.SifDrzave)
            {
                return BadRequest();
            }

            _context.Entry(drzava).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrzavaExists(id))
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



        // DELETE: api/Drzave/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var drzava = await _context.Drzavas.FindAsync(id);
            if (drzava == null)
            {
                return NotFound();
            }

            _context.Drzavas.Remove(drzava);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DrzavaExists(int id)
        {
            return _context.Drzavas.Any(e => e.SifDrzave == id);
        }
    }
}
