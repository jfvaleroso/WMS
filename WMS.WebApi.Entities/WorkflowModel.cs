using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.WebApi.Entities
{
    public class WorkflowModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string SubProcessCode { get; set; }
        public string SubProcessName { get; set; }
        public string ClassificationCode { get; set; }
        public string ClassificationName { get; set; }
        public bool Active { get; set; }
    }
}
