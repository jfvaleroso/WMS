using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Helper.Common;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Helper.Transaction;
using WMS.Web.Filter;
using WMS.Web.Helper;
using WMS.Web.Models;
using WMS.Webservice.IServices;

namespace WMS.Web.Controllers
{
    public class WorkflowMappingController : Controller
    {
        #region Constructor
        private readonly IWorkflowService workflowService;
        private readonly IRoleService roleService;
        private readonly IWorkflowMappingService workflowMappingService;
        private readonly IProcessService processService;
        private readonly Service service;
        public WorkflowMappingController(IWorkflowService workflowService, IRoleService roleService, IWorkflowMappingService workflowMappingService, IProcessService processService)
        {
            this.workflowService = workflowService;
            this.roleService = roleService;
            this.workflowMappingService = workflowMappingService;
            this.processService = processService;
            this.service = new Service(this.workflowService, roleService);
        }
        #endregion
        #region Index
        [CryptoValueProvider]
        public ActionResult Index(long id)
        {
            try
            {
                WorkflowMappingModel model = new WorkflowMappingModel();
                Workflow workflow = this.workflowService.GetDataById(id);
                model.WorkflowId = id;
                model.WorkflowList = this.service.GetWorkflowList(id,id);  
                model.RoleList = this.service.GetRoleList(0, workflow.Process.SystemCode);
                model.OperatorList = this.service.GetOperatorList("OR", "AND");
                model.WorkflowMappingList = GetCurrentWorkflowMapping() ?? null;
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
        public ActionResult SaveWorkflowMapping(WorkflowMappingModel model)
        {
            try
            {
                List<WorkflowMapping> currentWorkflowMapping = GetCurrentWorkflowMapping();
                if (currentWorkflowMapping.Any())
                {
                    foreach (var workflow in currentWorkflowMapping)
                    {
                        bool ifExists = this.workflowMappingService.CheckDataIfExists(workflow);
                         if (!ifExists)
                         {
                             workflow.CreatedBy = User.Identity.Name;
                             workflow.DateCreated = DateTime.Now;
                             workflow.Active = true;
                             this.workflowMappingService.Save(workflow);
                         }
                    }
                    ClearSession();
                    return Json(new { result = Base.Encrypt(model.WorkflowId.ToString()), message = MessageCode.saved, code = StatusCode.saved, content = MessageCode.saved });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid, content = MessageCode.error });
            }
            catch (Exception ex)
            {
                return Json(new { result = MessageCode.saved, message = MessageCode.saved, code = StatusCode.saved, content = ex.Message.ToString() });
            }
           
        }
        #endregion
        #region workflow

        [HttpPost]
        public ActionResult AddWorkflow(WorkflowMappingModel model)
        {
            if (ModelState.IsValid)
            {
                List<WorkflowMapping> workflowMappingList = new List<WorkflowMapping>();
                WorkflowMapping workflowMapping = new WorkflowMapping();
                List<WorkflowMapping> currentWorkflowMapping = GetCurrentWorkflowMapping();
                workflowMapping.Approver = model.Assignee != null ? string.Join(",", model.Assignee) : string.Empty;
                workflowMapping.Operator = model.Operator;
                workflowMapping.AlertTo = model.NoticeTo != null ? string.Join(",", model.NoticeTo) : string.Empty;
                workflowMapping.SLA = model.SLA;
                workflowMapping.Workflow = this.workflowService.GetDataById(model.WorkflowId);
                int counter = currentWorkflowMapping.Count();
                if (currentWorkflowMapping.Any())
                {
                    if (!currentWorkflowMapping.Exists(x => x.Approver.Equals(workflowMapping.Approver)))
                    {
                        workflowMapping.LevelId = counter++;
                        model.WorkflowMappingList.Add(workflowMapping);
                        currentWorkflowMapping.Add(workflowMapping);
                    }
                    else
                    {
                        ModelState.AddModelError("StatusError", "already added");
                        return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                    }

                }
                else
                {
                    workflowMapping.LevelId = counter++;
                    currentWorkflowMapping.Add(workflowMapping);
                }
                model.WorkflowMappingList = currentWorkflowMapping;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Partial/Workflow", model);
                }
                return Json(new { result = StatusCode.saved, message = MessageCode.saved, code = StatusCode.saved });
            }
            return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
        }
        public ActionResult RemoveWorkflow(string approver)
        {

            WorkflowMappingModel model = new WorkflowMappingModel();
            List<WorkflowMapping> currentWorkflowMapping = RemoveItemFromCurrentWorkflowMapping(approver);
            model.WorkflowMappingList = currentWorkflowMapping;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Partial/Workflow", model);
            }
            return View(model);
        }
        public ActionResult RearrangeWorkflow(string id)
        {

            WorkflowMappingModel model = new WorkflowMappingModel();
            List<WorkflowMapping> currentWorkflowMapping = RearrangeCurrentWorkflowMapping(id);
            model.WorkflowMappingList = currentWorkflowMapping;
            SetCurrentWorkflowMapping(currentWorkflowMapping);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Partial/Workflow", model);
            }
            return Json(new { result = StatusCode.loaded, message = MessageCode.valid, code = StatusCode.loaded });
        }
        #endregion
        #region private method
        private List<WorkflowMapping> GetCurrentWorkflowMapping()
        {
            var worklfowMapping = (List<WorkflowMapping>)Session["worklfowMapping"];
            if (worklfowMapping == null)
            {
                Session["worklfowMapping"] = worklfowMapping = new List<WorkflowMapping>();
            }
            return worklfowMapping;
        }
        private void SetCurrentWorkflowMapping(List<WorkflowMapping> noticationMappingList)
        {
            Session["worklfowMapping"] = noticationMappingList;
        }
        private List<WorkflowMapping> GetCurrentWorkflowMapping(long id)
        {
            Workflow workflow = this.workflowService.GetDataById(id);
            List<WorkflowMapping> workflowMappingList = this.workflowMappingService.GetFilteredDataByWorkflow(workflow);
            var workflowMapping = (List<WorkflowMapping>)Session["workflowMapping"];
            if (workflowMappingList != null)
            {
                Session["workflowMapping"] = workflowMapping = workflowMappingList;
            }
            return workflowMapping;
        }
        private List<WorkflowMapping> RemoveItemFromCurrentWorkflowMapping(string approver)
        {
            List<WorkflowMapping> currentWorkflowMapping = GetCurrentWorkflowMapping();
            currentWorkflowMapping.RemoveAll(x => x.Approver.Equals(approver));
            return currentWorkflowMapping;
        }
        private List<WorkflowMapping> RearrangeCurrentWorkflowMapping(string id)
        {
            List<WorkflowMapping> currentWorkflowMapping = GetCurrentWorkflowMapping();
            List<int> newOrder = id.Split(',').Select(n => int.Parse(n)).ToList();
            List<WorkflowMapping> workflowMappingList =  newOrder.Select(i => currentWorkflowMapping[i]).ToList();
            return workflowMappingList;
        }
        private WorkflowMapping GetItemFromCurrentWorkflowMapping(string approver)
        {
            List<WorkflowMapping> currentWorkflowMapping = GetCurrentWorkflowMapping();
            WorkflowMapping workflowMapping = currentWorkflowMapping.Find(x => x.Approver.Equals(approver));
            return workflowMapping;
        }
        private void PopulateCurrentWorkflowMapping(List<WorkflowMapping> workflowMappingList)
        {
            Session["workflowMapping"] = workflowMappingList;
        }
        private void ClearSession()
        {
            Session["workflowMapping"] = null;
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
                    WorkflowMappingModel model = new WorkflowMappingModel();
                    Workflow workflow = this.workflowService.GetDataById(id);
                    model.WorkflowId = id;
                    model.WorkflowList = this.service.GetWorkflowList(id, id);
                    model.RoleList = this.service.GetRoleList(0, workflow.Process.SystemCode);
                    model.OperatorList = this.service.GetOperatorList("OR", "AND");
                    model.SecuredId = Base.Encrypt(id.ToString());
                    model.WorkflowMappingList = GetCurrentWorkflowMapping(id) ?? GetCurrentWorkflowMapping();
                    return View(model);
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("Index", "Workflow");
        }
        public ActionResult SaveChangesInWorkflowMapping(WorkflowMappingModel model)
        {
            try
            {
                List<WorkflowMapping> currentWorkflowMapping = GetCurrentWorkflowMapping();
                if (currentWorkflowMapping.Any())
                {
                    //remove previous WorkflowMapping
                    Workflow workflow = this.workflowService.GetDataById(model.WorkflowId);
                    this.workflowMappingService.DeleteWorkflowMapping(workflow);

                    //add new workflow mapping
                    foreach (var item in currentWorkflowMapping)
                    {
                        bool ifExists = this.workflowMappingService.CheckDataIfExists(item);
                        if (!ifExists)
                        {
                            item.CreatedBy = User.Identity.Name;
                            item.DateCreated = DateTime.Now;
                            item.Active = true;
                            this.workflowMappingService.Save(item);
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
