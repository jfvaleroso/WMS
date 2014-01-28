using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Entities;

namespace WMS.Web.Models
{
    public class DocumentMappingModel : DocumentMapping
    {
        [Required]
        public long WorkflowId { get; set; }
        public int DocumentId { get; set; }
        public string SecuredId { get; set; }
        public IList<SelectListItem> WorkflowList { get; set; }
        public IList<SelectListItem> DocumentList { get; set; }
        public IList<DocumentMapping> DocumentMappingList { get; set; }

        public DocumentMappingModel()
        {
            this.DocumentList = new List<SelectListItem>();
            this.WorkflowList = new List<SelectListItem>();
            this.DocumentMappingList = new List<DocumentMapping>();
        }
    }
}