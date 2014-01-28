using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Entities;
using System.Web.Mvc;

namespace WMS.Web.Areas.Setting.Models
{
    public class SubProcessModel : SubProcess
    {
        
        public int ProcessId { get; set; }
        public List<SelectListItem> ProcessList { get; set; }
        public SubProcessModel()
        {
            this.ProcessList = new List<SelectListItem>();
        }
    }
}