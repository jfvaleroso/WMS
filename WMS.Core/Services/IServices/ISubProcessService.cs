using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Services.IServices
{
    public interface ISubProcessService:IService<SubProcess, int>, ISearchService<SubProcess, int>
    {
    }
}
