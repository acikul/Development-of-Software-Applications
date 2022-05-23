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
    public class GodineController : ControllerBase, ICustomController<int, Godina>
    {
        private readonly RPPP23Context _context;
        private static Dictionary<string, Expression<Func<Godina, object>>> orderSelectors = new Dictionary<string, Expression<Func<Godina, object>>>
        {
            [nameof(Godina.SifGodine).ToLower()] = m => m.SifGodine,
            [nameof(Godina.VrijednostGodine).ToLower()] = m => m.VrijednostGodine

        };

        public GodineController(RPPP23Context context)
        {
            _context = context;
        }


        [HttpGet("count", Name = "BrojGodina")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = _context.Godinas.AsQueryable();

            int count = await query.CountAsync();
            return count;
        }

        /// <summary>
        /// Dohvat svih Godina.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "DohvatiGodine")]
        public virtual async Task<List<Godina>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = _context.Godinas.AsQueryable();
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
        /// Dohvat odredene Godine
        /// </summary>
        /// <param name="id">Id Godine koju trazimo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Godina>> Get(int id)
        {
            var godina = await _context.Godinas.FindAsync(id);

            if (godina == null)
            {
                return NotFound();
            }

            return godina;
        }

        /// <summary>
        /// Kreiranje Godine
        /// </summary>
        /// <param name="Godina">Godina koju kreiramo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Godina godina)
        {

            _context.Godinas.Add(godina);

            await _context.SaveChangesAsync();
            var addedGodina = await Get(godina.SifGodine);

            return CreatedAtAction("GetGodina", new { id = godina.SifGodine }, addedGodina);
        }


        /// <summary>
        /// Azuriranje Godine
        /// </summary>
        /// <param name="id">Id Godine koju azurirmo</param>
        /// <param name="Godina">Atributi Godine koju azuriramo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Godina godina)
        {
            if (id != godina.SifGodine)
            {
                return BadRequest();
            }

            _context.Entry(godina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GodinaExists(id))
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



        // DELETE: api/Godine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var godina = await _context.Godinas.FindAsync(id);
            if (godina == null)
            {
                return NotFound();
            }

            _context.Godinas.Remove(godina);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GodinaExists(int id)
        {
            return _context.Godinas.Any(e => e.SifGodine == id);
        }
    }
}
