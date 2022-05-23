using JavnaNabava.Models;
using JavnaNabava.Util.ExceptionFilters;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    /// <summary>
    /// Web API servis za rad s ovlastenicima
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class OvlasteniciController : ControllerBase, ICustomController<string, Ovlastenik>
    {
        private readonly RPPP23Context _context;
        private static Dictionary<string, Expression<Func<Ovlastenik, object>>> orderSelectors = new Dictionary<string, Expression<Func<Ovlastenik, object>>>
        {
            [nameof(Ovlastenik.Oibovlastenika).ToLower()] = m => m.Oibovlastenika,
            [nameof(Ovlastenik.ImeOvlastenika).ToLower()] = m => m.ImeOvlastenika,
            [nameof(Ovlastenik.PrezimeOvlastenika).ToLower()] = m => m.PrezimeOvlastenika,
            [nameof(Ovlastenik.FunkcijaOvlastenika).ToLower()] = m => m.FunkcijaOvlastenika,
            [nameof(Ovlastenik.Oibnarucitelj).ToLower()] = m => m.Oibnarucitelj


        };

        public OvlasteniciController(RPPP23Context context)
        {
            _context = context;
        }


        [HttpGet("count", Name = "BrojOvlastenika")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = _context.Ovlasteniks.AsQueryable();

            int count = await query.CountAsync();
            return count;
        }

        /// <summary>
        /// Dohvat svih ovlastenika.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "DohvatiOvlastenike")]
        public virtual async Task<List<Ovlastenik>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = _context.Ovlasteniks.AsQueryable();
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
        /// Dohvat odredenog ovlastenika
        /// </summary>
        /// <param name="id">oib ovlastenika kojeg trazimo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Ovlastenik>> Get(string id)
        {
            var ovlastenik = await _context.Ovlasteniks.FindAsync(id);

            if (ovlastenik == null)
            {
                return NotFound();
            }

            return ovlastenik;
        }

        /// <summary>
        /// Kreiranje ovlastenika
        /// </summary>
        /// <param name="ovlastenik">Ovlastenik kojeg kreiramo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Ovlastenik ovlastenik)
        {

            _context.Ovlasteniks.Add(ovlastenik);

            await _context.SaveChangesAsync();
            var addedOvlastenik = await Get(ovlastenik.Oibovlastenika);

            return CreatedAtAction("GetOvlastenik", new { id = ovlastenik.Oibovlastenika }, addedOvlastenik);
        }


        /// <summary>
        /// Azuriranje ovlastenika
        /// </summary>
        /// <param name="id">Oib ovlastenika kojeg azurirmo</param>
        /// <param name="ovlastenik">Atributi ovlastenika kojeg azuriramo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Ovlastenik ovlastenik)
        {
            if (id != ovlastenik.Oibovlastenika)
            {
                return BadRequest();
            }

            _context.Entry(ovlastenik).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OvlastenikExists(id))
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


       // DELETE: api/Ovlastenici/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ovlastenik = await _context.Ovlasteniks.FindAsync(id);
            if (ovlastenik == null)
            {
                return NotFound();
            }

            _context.Ovlasteniks.Remove(ovlastenik);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OvlastenikExists(string id)
        {
            return _context.Ovlasteniks.Any(e => e.Oibovlastenika == id);
        }
    }

   
    }



