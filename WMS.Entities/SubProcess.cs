using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WMS.Entities
{
    public class SubProcess : Entity<int> 
    {
        public virtual Process Process { get; set; }
        [Required]
        public virtual string Code { get; set; }
        [Required]
        public virtual string Name { get; set; }
         [Required]
        public virtual string Description { get; set; }
        public virtual IList<Classification> Classifications { get; set; }
        public virtual IList<Workflow> Workflows { get; set; }
        public SubProcess()
        {
            this.Classifications = new List<Classification>();
            this.Workflows = new List<Workflow>();

        }
     
    }
}
