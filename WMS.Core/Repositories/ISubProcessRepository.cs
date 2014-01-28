using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Repositories
{
    public interface ISubProcessRepository : IRepository<SubProcess, int>,ISearchRepository<SubProcess>
    {
    }
}
