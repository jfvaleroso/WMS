using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;
using WMS.Core.Repositories;
using NHibernate.Criterion;
using WMS.Core.Helper.Common;


namespace WMS.NHibernateBase.Repositories
{
    public class NHWorkflowRepository : NHRepositoryBase<Workflow, long>, IWorkflowRepository
    {
        public List<Workflow> GetDataWithPagingAndSearch(string searchString, int pageIndex, int pageSize, out long total)
        {
            List<ICriterion> criterion = new List<ICriterion>();
            criterion.Add(Restrictions.Or(
               NHibernate.Criterion.Restrictions.Like("ModifiedBy", Base.SearchString(searchString)),
               NHibernate.Criterion.Restrictions.Like("CreatedBy", Base.SearchString(searchString)
               )));
            return this.GetDataWithPagingAndSearch(criterion, searchString, pageIndex, pageSize, out total);
        }

    }
}
