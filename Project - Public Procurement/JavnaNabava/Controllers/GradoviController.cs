using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using JavnaNabava.ViewModels;
using JavnaNabava.Util.ExceptionFilters;

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
    public class GradoviController : ControllerBase, ICustomController<int, Grad>
    {
        private readonly RPPP23Context _context;
        private static Dictionary<string, Expression<Func<Grad, object>>> orderSelectors = new Dictionary<string, Expression<Func<Grad, object>>>
        {
            [nameof(Grad.SifGrada).ToLower()] = m => m.SifGrada,
            [nameof(Grad.NazivGrada).ToLower()] = m => m.NazivGrada,
            [nameof(Grad.SifDrzava).ToLower()] = m => m.SifDrzava

        };

        public GradoviController(RPPP23Context context)
        {
            _context = context;
        }


        [HttpGet("count", Name = "BrojGradova")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = _context.Grads.AsQueryable();

            int count = await query.CountAsync();
            return count;
        }

        /// <summary>
        /// Dohvat svih gradova.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "DohvatiGradove")]
        public virtual async Task<List<Grad>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = _context.Grads.AsQueryable();
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
        /// Dohvat odredenog grada
        /// </summary>
        /// <param name="id">Id grada kojeg trazimo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Grad>> Get(int id)
        {
            var grad = await _context.Grads.FindAsync(id);

            if (grad == null)
            {
                return NotFound();
            }

            return grad;
        }

        /// <summary>
        /// Kreiranje grada
        /// </summary>
        /// <param name="grad">Grad koji kreiramo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Grad grad)
        {

            _context.Grads.Add(grad);

            await _context.SaveChangesAsync();
            var addedGrad = await Get(grad.SifGrada);

            return CreatedAtAction("GetGrad", new { id = grad.SifGrada }, addedGrad);
        }


        /// <summary>
        /// Azuriranje grada
        /// </summary>
        /// <param name="id">Id grada kojeg azurirmo</param>
        /// <param name="grad">Atributi grada kojeg azuriramo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Grad grad)
        {
            if (id != grad.SifGrada)
            {
                return BadRequest();
            }

            _context.Entry(grad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradExists(id))
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



        // DELETE: api/Grad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var grad = await _context.Grads.FindAsync(id);
            if (grad == null)
            {
                return NotFound();
            }

            _context.Grads.Remove(grad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradExists(int id)
        {
            return _context.Grads.Any(e => e.SifGrada == id);
        }
    }
}