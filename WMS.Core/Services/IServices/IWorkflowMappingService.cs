using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Services.IServices
{
    public interface IWorkflowMappingService : IService<WorkflowMapping, long>
    {
        //get data by workflow ID
        List<WorkflowMapping> GetFilteredDataByWorkflow(Workflow workflow);
        WorkflowMapping GetCurrentApproverByWorkflowAndLevel(Workflow workflow, int level);

        void DeleteWorkflowMapping(Workflow workflow);

    }
}
