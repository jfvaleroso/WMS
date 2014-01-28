using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Services.IServices
{
    public interface IWorkflowService : IService<Workflow, long>, ISearchService<Workflow, long>
    {
        Workflow GetWorkflowByCode(string code);
    }
}
