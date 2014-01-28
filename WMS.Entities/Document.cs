using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WMS.Entities
{
    public class Document : Entity<int>
    {

        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Code { get; set; }
        [Required]
        public virtual string Description { get; set; }
        public virtual IList<DocumentMapping> DocumentMappings { get; set; }
        public Document()
        {

            this.DocumentMappings = new List<DocumentMapping>();
        }
    }
}
