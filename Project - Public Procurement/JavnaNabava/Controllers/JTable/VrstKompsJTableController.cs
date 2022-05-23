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
    [Route("jtable/vrstKomp/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class VrstKompsJTableController : JTableController<VrstKompsController, int, VrstaKompetencije>
    {
        public VrstKompsJTableController(VrstKompsController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] VrstaKompetencije model)
        {
            return await base.UpdateItem(model.SifVrsteKompetencije, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int SifVrsteKompetencije)
        {
            return await base.DeleteItem(SifVrsteKompetencije);
        }
    }
}
