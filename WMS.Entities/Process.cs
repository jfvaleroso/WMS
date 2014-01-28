using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WMS.Entities
{
    public class Process : Entity<int> 
    {
        [Required]
        public virtual string SystemCode { get; set; }
        [Required]
        public virtual string Code { get; set; }
        [Required]
        public virtual string Name { get; set; }
         [Required]
        public virtual string Description { get; set; }
        public virtual IList<SubProcess> SubProcesses { get; set; }
        public virtual IList<Workflow> Workflows { get; set; }
        public Process()
        {
            this.SubProcesses = new List<SubProcess>();
            this.Workflows = new List<Workflow>();
        }
       
      
    }
}
