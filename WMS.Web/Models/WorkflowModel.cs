using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Entities;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WMS.Web.Models
{
    public class WorkflowModel : Workflow
    {
        [Required]
        [Display(Name="Process Code")]
        public int ProcessId { get; set; }
        public int? SubProcessId { get; set; }
        public int? ClassificationId { get; set; }
        [Required]
        [Display(Name = "Requestor")]
        public string[] RequestorCode { get; set; }
       
        public IList<SelectListItem> SubProcessList { get; set; }
        public IList<SelectListItem> ProcessList { get; set; }
        public IList<SelectListItem> ClassificationList { get; set; }
        public IList<SelectListItem> RoleList { get; set; }

        public WorkflowModel()
        {
            this.SubProcessList = new List<SelectListItem>();
            this.ProcessList = new List<SelectListItem>();
            this.ClassificationList = new List<SelectListItem>();
            this.RoleList = new List<SelectListItem>();
        }

    }
}