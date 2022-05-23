using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JavnaNabava.Util.ExceptionFilters;
using JavnaNabava.Models;
using JavnaNabava.ViewModels.JTable;

namespace JavnaNabava.Controllers.JTable
{
    [Route("jtable/kriterij/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class KriterijiJTableController : JTableController<KriterijiController, int, Kriterij>
    {
        public KriterijiJTableController(KriterijiController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] Kriterij model)
        {
            return await base.UpdateItem(model.SifKriterija, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int SifKriterija)
        {
            return await base.DeleteItem(SifKriterija);
        }
    }
}
