using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Repositories;
using WMS.Entities;

namespace WMS.NHibernateBase.Repositories
{
    public class NHWorkflowMappingRepository : NHRepositoryBase<WorkflowMapping, long>, IWorkflowMappingRepository
    {
        public List<WorkflowMapping> GetDataWithPagingAndSearch(string searchString, int pagNumber, int pageSize, out long total)
        {
            throw new NotImplementedException();
        }
    }
}
