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
    [Route("jtable/grad/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class GradoviJTableController : JTableController<GradoviController, int, Grad>
    {
        public GradoviJTableController(GradoviController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] Grad model)
        {
            return await base.UpdateItem(model.SifGrada, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int SifGrada)
        {
            return await base.DeleteItem(SifGrada);
        }
    }
}