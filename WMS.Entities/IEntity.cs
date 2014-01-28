using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
