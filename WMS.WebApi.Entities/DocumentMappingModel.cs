using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.WebApi.Entities
{
    public class DocumentMappingModel
    {

        public long WorkflowId { get; set; }
        public string WorkflowCode { get; set; }

        public bool Mandatory { get; set; } 
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
