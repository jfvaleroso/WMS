using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Entities
{
    //common ID
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
        public virtual bool Active { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? DateModified { get; set; }

        public Entity()
        {
            this.Active = true;
     
        }
    }
}
