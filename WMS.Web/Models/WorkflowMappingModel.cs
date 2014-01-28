using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Entities;

namespace WMS.Web.Models
{
    public class WorkflowMappingModel : WorkflowMapping
    {
        [Required]
        [Display(Name="Workflow")]
        public long WorkflowId { get; set; }
         [Required]
        public string[] Assignee { get; set; }
        public string[] NoticeTo { get; set; }
        public string SecuredId { get; set; }
        public IList<SelectListItem> WorkflowList { get; set; }
        public IList<SelectListItem> RoleList { get; set; }
        public IList<SelectListItem> OperatorList { get; set; }
        public IList<WorkflowMapping> WorkflowMappingList { get; set; }

        public WorkflowMappingModel()
        {
            this.WorkflowList = new List<SelectListItem>();
            this.RoleList = new List<SelectListItem>();
            this.WorkflowMappingList = new List<WorkflowMapping>();
        }
    }
}