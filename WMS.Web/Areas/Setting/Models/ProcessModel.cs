using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Entities;

namespace WMS.Web.Areas.Setting.Models
{
    public class ProcessModel : Process
    {
        public IList<SelectListItem> SystemList { get; set; }

        public ProcessModel()
        {
            this.SystemList = new List<SelectListItem>();
        }
    }
}