using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Core.Services.IServices
{
    public interface ISearchService<TEntity, TPrimaryKey>
    {
        List<TEntity> GetDataListWithFilter(bool isActive, TPrimaryKey filter);
    }
}
