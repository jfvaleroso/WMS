using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Entities;
using System.Web.Mvc;

namespace WMS.Web.Areas.Setting.Models
{
    public class ClassificationModel : Classification
    {
        public int SubProcessId { get; set; }
        public List<SelectListItem> SubProcessList { get; set; }
        public ClassificationModel()
        {
            this.SubProcessList = new List<SelectListItem>();
        }
    }
}