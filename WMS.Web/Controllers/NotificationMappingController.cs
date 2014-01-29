using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Services.IServices;
using WMS.Web.Helper;
using WMS.Web.Filter;
using WMS.Web.Models;
using WMS.Entities;
using WMS.Core.Helper.Common;
using WMS.Helper.Transaction;

namespace WMS.Web.Controllers
{
    public class NotificationMappingController : Controller
    {
        #region Constructor
        private readonly IWorkflowService workflowService;
        private readonly IStatusService statusService;
        private readonly INotificationMappingService notificationMappingService;
        private readonly Service service;
        public NotificationMappingController(IWorkflowService workflowService, IStatusService statusService, INotificationMappingService notificationMappingService)
        {
            this.workflowService = workflowService;
            this.statusService = statusService;
            this.notificationMappingService = notificationMappingService;
            this.service = new Service(this.statusService, this.workflowService);
        }
        #endregion


        #region Index
        [CryptoValueProvider]
        public ActionResult Index(long id)
        {
            try
            {
                NotificationMappingModel model = new NotificationMappingModel();
                model.WorkflowId = id;
                model.WorkflowList = this.service.GetWorkflowList(id, id);
                model.StatusList = this.service.GetStatusList(true);
                model.NotificationMappingList = GetCurrentNotificationMapping() ?? null;
                if (model != null)
                    return View(model);
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Save
        [HttpPost]
        public ActionResult SaveNotification(NotificationMappingModel model)
        {
            try
            {
                List<NotificationMapping> currentNotificationMapping = GetCurrentNotificationMapping();
                if (currentNotificationMapping.Any())
                {
                    foreach (var notification in currentNotificationMapping)
                    {
                        bool ifExists = this.notificationMappingService.CheckDataIfExists(notification);
                        if (!ifExists)
                        {
                            notification.CreatedBy = User.Identity.Name;
                            notification.DateCreated = DateTime.Now;
                            notification.Active = true;
                            this.notificationMappingService.Save(notification);
                        }
                        return Json(new { result = string.Empty, message = MessageCode.saved, code = StatusCode.saved, content = MessageCode.saved });
                    }
                    ClearSession();
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid, content = MessageCode.error });
            }
            catch (Exception ex)
            {
                return Json(new { result = MessageCode.error, message = MessageCode.error, code = StatusCode.invalid, content = ex.Message.ToString() });
            }
           
        }
        #endregion
        #region notification
     
        [HttpPost]
        public ActionResult AddNotification(NotificationMappingModel model)
        {
            if (ModelState.IsValid)
            {
                List<NotificationMapping> notificationMappingList = new List<NotificationMapping>();
                NotificationMapping notificationMapping = new NotificationMapping();
                List<NotificationMapping> currentNotificationMapping = GetCurrentNotificationMapping();
                notificationMapping.EmailContent = model.EmailContent;
                notificationMapping.SMSContent = model.SMSContent;
                notificationMapping.Status = this.statusService.GetDataById(model.StatusId);
                notificationMapping.Workflow = this.workflowService.GetDataById(model.WorkflowId);
                if (currentNotificationMapping.Any())
                {
                    if (!currentNotificationMapping.Exists(x => x.Status.Id.Equals(notificationMapping.Status.Id)))
                    {
                        model.NotificationMappingList.Add(notificationMapping);
                        currentNotificationMapping.Add(notificationMapping);
                    }
                    else
                    {
                        ModelState.AddModelError("StatusError", "already added");
                        return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                    }

                }
                else
                {
                    currentNotificationMapping.Add(notificationMapping);
                }
                model.NotificationMappingList = currentNotificationMapping;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Partial/Notification", model);
                }
                return Json(new { result = StatusCode.saved, message = MessageCode.saved, code = StatusCode.saved });
            }
            return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
        }        
        public ActionResult RemoveNotification(string code)
        {

            NotificationMappingModel model = new NotificationMappingModel();
            List<NotificationMapping> currentNotificationMapping = RemoveItemFromCurrentNotificationMapping(code);
            model.NotificationMappingList = currentNotificationMapping;
            if(Request.IsAjaxRequest())
            { 
              return PartialView("Partial/Notification", model);
            }
            return View(model);
        }
      
        #endregion
        #region private method
        private List<NotificationMapping> GetCurrentNotificationMapping()
        {
            var notificationMapping = (List<NotificationMapping>)Session["notificationMapping"];
            if (notificationMapping == null)
            {
                Session["notificationMapping"] = notificationMapping = new List<NotificationMapping>();
            }
            return notificationMapping;
        }
        private List<NotificationMapping> GetCurrentNotificationMapping(long id)
        {
            Workflow workflow = this.workflowService.GetDataById(id);
            List<NotificationMapping> notificationMappingList = this.notificationMappingService.GetFilteredDataByWorkflow(workflow);
            var notificationMapping = (List<NotificationMapping>)Session["notificationMapping"];
            if (notificationMappingList != null)
            {
                Session["notificationMapping"] = notificationMapping = notificationMappingList;
            }
            return notificationMapping;
        }
        private List<NotificationMapping> RemoveItemFromCurrentNotificationMapping(string code)
        {
            List<NotificationMapping> currentNotificationMapping = GetCurrentNotificationMapping();
            currentNotificationMapping.RemoveAll(x => x.Status.Code.Equals(code));
            return currentNotificationMapping;
        }
        private NotificationMapping GetItemFromCurrentNotificationMapping(string code)
        {
            List<NotificationMapping> currentNotificationMapping = GetCurrentNotificationMapping();
            NotificationMapping notificationMapping = currentNotificationMapping.Find(x => x.Status.Code.Equals(code));
            return notificationMapping;
        }
        private void PopulateCurrentNotificationMapping(List<NotificationMapping> noticationMappingList)
        {
            Session["notificationMapping"] = noticationMappingList;
        }
        private void ClearSession()
        {
            Session["notificationMapping"] = null;
        }
        #endregion
        #region Modify from List
        public ActionResult Modify(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                NotificationMappingModel model = new NotificationMappingModel();
                NotificationMapping currentNotificationMapping = GetItemFromCurrentNotificationMapping(code);
                model.EmailContent = currentNotificationMapping.EmailContent;
                model.SMSContent = currentNotificationMapping.SMSContent;
                model.Status = currentNotificationMapping.Status;
                model.Workflow = currentNotificationMapping.Workflow;
                return View(model);
            }
            return RedirectToAction("Index", "Workflow");
        }
        [HttpPost]
        public ActionResult Modify(NotificationMappingModel model)
        {
            if (model != null)
            {
                List<NotificationMapping> currentNotificationMapping = GetCurrentNotificationMapping();
                NotificationMapping notificationMapping = GetItemFromCurrentNotificationMapping(model.Status.Code);
                notificationMapping.EmailContent = model.EmailContent;
                notificationMapping.SMSContent = model.SMSContent;
                notificationMapping.Status = model.Status;
                notificationMapping.Workflow = model.Workflow;
                RemoveItemFromCurrentNotificationMapping(model.Status.Code);
                currentNotificationMapping.Add(notificationMapping);
                PopulateCurrentNotificationMapping(currentNotificationMapping);
                return View(model);

            }
            return View();
        }
        #endregion
        #region Manage
        [CryptoValueProvider]
        public ActionResult Manage(long id)
        {
            if (id != 0)
            {
                try
                {
                    NotificationMappingModel model = new NotificationMappingModel();
                    model.WorkflowId = id;
                    model.WorkflowList = this.service.GetWorkflowList(id, id);
                    model.StatusList = this.service.GetStatusList(true);
                    model.SecuredId = Base.Encrypt(id.ToString());
                    model.NotificationMappingList = GetCurrentNotificationMapping(id) ?? GetCurrentNotificationMapping();
                    return View(model);
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("Index", "Workflow");
        }
        public ActionResult SaveChangesInNotificationMapping(NotificationMappingModel model)
        {
            try
            {
                List<NotificationMapping> currentNotificationMapping = GetCurrentNotificationMapping();
                if (currentNotificationMapping.Any())
                {
                    //remove previous NotificationMapping
                    Workflow workflow = this.workflowService.GetDataById(model.WorkflowId);
                    this.notificationMappingService.DeleteNotificationMapping(workflow);

                    //add new document mapping
                    foreach (var notification in currentNotificationMapping)
                    {
                        bool ifExists = this.notificationMappingService.CheckDataIfExists(notification);
                        if (!ifExists)
                        {
                            notification.CreatedBy = User.Identity.Name;
                            notification.DateCreated = DateTime.Now;
                            notification.Active = true;
                            this.notificationMappingService.Save(notification);
                        }
                    }
                    ClearSession();
                    return Json(new { result = Base.Encrypt(model.WorkflowId.ToString()), message = MessageCode.saved, code = StatusCode.saved, content = string.Empty });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid, content = MessageCode.error });
            }
            catch (Exception ex)
            {
                return Json(new { result = MessageCode.error, message = MessageCode.error, code = StatusCode.invalid, content = ex.Message.ToString() });
            }
        }
        #endregion


    }
}
