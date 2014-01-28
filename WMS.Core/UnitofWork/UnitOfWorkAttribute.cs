using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Core.UnitofWork
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {

    }
}
