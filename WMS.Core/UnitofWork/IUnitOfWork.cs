using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Core.UnitofWork
{
    public interface IUnitOfWork
    {

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
