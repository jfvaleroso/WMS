using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Services.IServices
{
    public interface INotificationMappingService : IService<NotificationMapping, long>
    {
        List<NotificationMapping> GetByExpression(long id);
        //get data by workflow ID
        List<NotificationMapping> GetFilteredDataByWorkflow(Workflow workflow);
        NotificationMapping GetNotificationMappingByWorkflowAndStatus(Workflow workflow, Status status);
        void DeleteNotificationMapping(Workflow workflow);
    }
}
