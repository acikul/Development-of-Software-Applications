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
    [Route("jtable/drzava/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class DrzaveJTableController : JTableController<DrzaveController, int, Drzava>
    {
        public DrzaveJTableController(DrzaveController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] Drzava model)
        {
            return await base.UpdateItem(model.SifDrzave, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int SifDrzave)
        {
            return await base.DeleteItem(SifDrzave);
        }
    }
}
