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
    [Route("jtable/godina/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class GodineJTableController : JTableController<GodineController, int, Godina>
    {
        public GodineJTableController(GodineController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] Godina model)
        {
            return await base.UpdateItem(model.SifGodine, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int SifGodine)
        {
            return await base.DeleteItem(SifGodine);
        }
    }
}
