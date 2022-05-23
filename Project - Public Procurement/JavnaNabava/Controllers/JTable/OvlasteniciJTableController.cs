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
    [Route("jtable/ovlastenik/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class OvlasteniciJTableController : JTableController<OvlasteniciController, string, Ovlastenik>
    {
        public OvlasteniciJTableController(OvlasteniciController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] Ovlastenik model)
        {
            return await base.UpdateItem(model.Oibovlastenika, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] string Oibovlastenika)
        {
            return await base.DeleteItem(Oibovlastenika);
        }
    }
}
