using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Entities
{
    public class DocumentMapping : Entity<long>
    {

        public virtual bool Mandatory { get; set; }
        public virtual Workflow Workflow { get; set; }
        public virtual Document Document { get; set; }

        public DocumentMapping()
        {
            this.Mandatory = true;
        }
    }
}
