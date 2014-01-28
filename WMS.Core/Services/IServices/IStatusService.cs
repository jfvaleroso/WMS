using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Services.IServices
{
    public interface IStatusService : IService<Status, int>, ISearchService<Status, int>
    {
    }
}
