using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Ovlastenik
    {
        public string Oibovlastenika { get; set; }
        public string ImeOvlastenika { get; set; }
        public string PrezimeOvlastenika { get; set; }
        public string FunkcijaOvlastenika { get; set; }
        public string Oibnarucitelj { get; set; }

        public virtual Narucitelj OibnaruciteljNavigation { get; set; }
    }
}
