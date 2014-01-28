using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.WebApi.Entities;

namespace WMS.WebApi.Controllers
{
    public class WorkflowController : ApiController
    {
         #region Constructor
        private readonly IWorkflowService workflowService;
        private readonly IDocumentMappingService documentMappingService;
        private readonly INotificationMappingService notificationMappingService;
        private readonly IWorkflowMappingService workflowMappingService;
        private readonly IStatusService statusService;
    


        public WorkflowController(IWorkflowService workflowService, IDocumentMappingService documentMappingService,
            INotificationMappingService notificationMappingService, IWorkflowMappingService workflowMappingService,
            IStatusService statusService)
        {
            this.workflowService = workflowService;
            this.documentMappingService = documentMappingService;
            this.notificationMappingService = notificationMappingService;
            this.workflowMappingService = workflowMappingService;  
        }
        #endregion


        #region NotificationMapping
        public IEnumerable<NotificationMappingModel> GetNotificationMappingByWorkflow(long id)
        {
           var workflow= this.workflowService.GetDataById(id);
           var notificationMappingList= this.notificationMappingService.GetFilteredDataByWorkflow(workflow);
           List<NotificationMappingModel> modelList = new List<NotificationMappingModel>();
           foreach (var item in notificationMappingList)
           {
               NotificationMappingModel model = new NotificationMappingModel();
               model.EmailContent = item.EmailContent;
               model.SMSContent = item.SMSContent;
               modelList.Add(model);
           }
           return modelList;
        }
        public NotificationMappingModel GetNotificationMappingByWorkflowAndStatus(long id, int status)
        {
            var workflow = this.workflowService.GetDataById(id);
            var stat= this.statusService.GetDataById(status);
            var notificationMapping = this.notificationMappingService.GetNotificationMappingByWorkflowAndStatus(workflow,stat);
            NotificationMappingModel model = new NotificationMappingModel();
            model.EmailContent= notificationMapping.EmailContent;
            model.SMSContent= notificationMapping.SMSContent;
            model.StatusCode= notificationMapping.Status.Code;
            return model;
        }
        #endregion
        #region Document Mapping
        public IEnumerable<DocumentMappingModel> GetDocumentMappingByWorkflow(long id)
        {
            var workflow = this.workflowService.GetDataById(id);
            var documentMappingList = this.documentMappingService.GetFilteredDataByWorkflow(workflow);
            List<DocumentMappingModel> modelList = new List<DocumentMappingModel>();
            foreach (var item in documentMappingList)
            {
                DocumentMappingModel model = new DocumentMappingModel();
                model.Code = item.Document.Code;
                model.Name = item.Document.Name;
                model.Description = item.Document.Description;
                model.Mandatory = item.Mandatory;
                model.WorkflowCode = item.Workflow.Code;
                model.WorkflowId = item.Workflow.Id;
                model.Active = item.Active;
                modelList.Add(model);
            }
            return modelList;
        }
        #endregion
        #region Workflow Mapping
        public IEnumerable<WorkflowMappingModel> GetWorkflowMappingByWorkflow(long id)
        {
            var workflow = this.workflowService.GetDataById(id);
            var workflowMappingList = this.workflowMappingService.GetFilteredDataByWorkflow(workflow);
            List<WorkflowMappingModel> modelList = new List<WorkflowMappingModel>();
            foreach (var item in workflowMappingList)
            {
                WorkflowMappingModel model = new WorkflowMappingModel();
                model.LevelId = item.LevelId;
                model.SLA = item.SLA;
                model.Operator = item.Operator;
                model.Approver = item.Approver;
                model.AlertTo = item.Approver;
                model.SMSNotification = item.SMSNotification;
                model.EmailNotification = item.EmailNotification;
                modelList.Add(model);
            }
            return modelList;
        }
        public WorkflowMappingModel GetWorkflowMappingByWorkflowAndLevel(long id,int level)
        {
            var workflow = this.workflowService.GetDataById(id);
            WorkflowMapping workflowMapping = this.workflowMappingService.GetCurrentApproverByWorkflowAndLevel(workflow, level);
            WorkflowMappingModel model = new WorkflowMappingModel();
            model.AlertTo = workflowMapping.AlertTo;
            model.Approver = workflowMapping.AlertTo;
            model.LevelId = workflowMapping.LevelId;
            model.Operator = workflowMapping.Operator;
            model.SLA = workflowMapping.SLA;
            model.SMSNotification = workflowMapping.SMSNotification;
            model.EmailNotification = workflowMapping.EmailNotification;
            return model;
        }
        #endregion
        #region Workflow
        public WorkflowModel GetWorkflowByCode(string code)
        {
            var workflow = this.workflowService.GetWorkflowByCode(code);
            WorkflowModel model = new WorkflowModel();
            model.ClassificationCode = workflow.Classification.Code;
            model.ClassificationName = workflow.Classification.Name;
            model.Code = workflow.Code;
            model.Description = workflow.Description;
            model.Name = workflow.Name;
            model.ProcessCode = workflow.Process.Code;
            model.ProcessName = workflow.Process.Name;
            model.SubProcessCode = workflow.SubProcess.Code;
            model.SubProcessName = workflow.SubProcess.Name;
            model.ClassificationCode = workflow.Classification.Code;
            model.ClassificationName = workflow.Classification.Name;
            model.Active = workflow.Active;
            return model;
        }
        #endregion








    }
}
