using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Core.Repositories
{
    public interface ISearchRepository<TEntity>
    {
        List<TEntity> GetDataWithPagingAndSearch(string searchString, int pagNumber, int pageSize, out long total);
        
        
    }
}
