using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Services.IServices
{
    public interface IClassificationService : IService<Classification, int>, ISearchService<Classification, int>
    {
    }
}
