using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.WebApi.Entities
{
    public class WorkflowMappingModel
    {
        public int LevelId { get; set; }
        public int SLA { get; set; }
        public string Operator { get; set; }
        public string Approver { get; set; }
        public string AlertTo { get; set; }
        public bool SMSNotification { get; set; }
        public bool EmailNotification { get; set; }
    }
}
