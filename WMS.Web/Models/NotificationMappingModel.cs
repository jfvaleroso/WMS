using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Entities;

namespace WMS.Web.Models
{
    public class NotificationMappingModel : NotificationMapping
    {
        [Required]
        public long WorkflowId { get; set; }
        [Required]
        [Display(Name="Status")]
        public int StatusId { get; set; }
        [AllowHtml]
        [Required]
        public override string EmailContent { get; set; }
        public string SecuredId { get; set; }
        public IList<SelectListItem> WorkflowList { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public IList<NotificationMapping> NotificationMappingList { get; set; }


        public NotificationMappingModel()
        {
            this.WorkflowList = new List<SelectListItem>();
            this.StatusList = new List<SelectListItem>();
            this.NotificationMappingList = new List<NotificationMapping>();
        }
    }
}