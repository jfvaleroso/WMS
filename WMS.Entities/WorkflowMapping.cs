using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WMS.Entities
{
    public class WorkflowMapping : Entity<long>
    {

        public virtual Workflow Workflow { get; set; }
        public virtual int LevelId { get; set; }
        [Range(1,300)]
        public virtual int SLA { get; set; }
        public virtual string Operator { get; set; }
        public virtual string Approver { get; set; }
        public virtual string AlertTo { get; set; }
        public virtual bool SMSNotification { get; set; }
        public virtual bool EmailNotification { get; set; }
      

        public WorkflowMapping()
        {
          
        }
    }
}
