using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WMS.Entities
{
    public class Workflow : Entity<long>
    {
        [Required]
        public virtual string Code { get; set; }
        [Required]
        public virtual string Name { get; set; }
         [Required]
        public virtual string Description { get; set; }
        public virtual Process Process { get; set; }
        public virtual SubProcess SubProcess { get; set; }
        public virtual Classification Classification { get; set; }
        public virtual IList<WorkflowMapping> WorkflowMappings { get; set; }
        public virtual IList<DocumentMapping> DocumentMappings { get; set; }
        public virtual IList<NotificationMapping> NotificationMappings { get; set; }
        public virtual string Requestor { get; set; }



        public Workflow()
        {
            this.WorkflowMappings = new List<WorkflowMapping>();
            this.DocumentMappings = new List<DocumentMapping>();
            this.NotificationMappings = new List<NotificationMapping>();
        }
    }
}
