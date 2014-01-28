using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Helper.Common;
using WMS.Core.Services.IServices;
using WMS.Web.Helper;
using WMS.Web.Filter;
using WMS.Web.Models;
using WMS.Entities;
using WMS.Helper.Transaction;
using WMS.Webservice.IServices;

namespace WMS.Web.Controllers
{
    public class WorkflowController : Controller
    {
        
        #region Constructor
        private readonly IWorkflowService workflowService;
        private readonly IProcessService processService;
        private readonly ISubProcessService subProcessService;
        private readonly IClassificationService classificationService;
        private readonly IRoleService roleService;
        private readonly Service service;
        public WorkflowController(IWorkflowService workflowService, IProcessService processService, ISubProcessService subProcessService, IClassificationService classificationService, IRoleService roleService)
        {
            this.workflowService = workflowService;
            this.processService = processService;
            this.subProcessService= subProcessService;;
            this.classificationService= classificationService;
            this.roleService = roleService;
            this.service = new Service(this.processService, this.subProcessService, this.classificationService, this.roleService);
        }
        #endregion
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult WorkflowListWithPaging(string searchString = "", int jtStartIndex = 1, int jtPageSize = 15)
        {
            try
            {
                long total = 0;
                var workflowList = !string.IsNullOrEmpty(searchString) ?
                    this.workflowService.GetDataListWithPagingAndSearch(searchString, jtStartIndex, jtPageSize, out total) :
                   this.workflowService.GetDataListWithPaging(jtStartIndex, jtPageSize, out total);
                var collection = workflowList.Select(x => new
                {
                    Id = x.Id,
                    SecuredId = Base.Encrypt(x.Id.ToString()),
                    Active = x.Active,
                    Code= x.Code,
                    Name= x.Name,
                    Description= x.Description,
                    ProcessName = x.Process.Name,
                    SubProcessName= x.SubProcess!=null ?  x.SubProcess.Name : string.Empty,
                    ClassificationName= x.Classification!=null ? x.Classification.Name : string.Empty
                });
                return Json(new { Result = "OK", Records = collection, TotalRecordCount = total }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "ERROR", Message = "error" });
            }
        }
        #endregion
        #region New
        public ActionResult New()
        {
            WorkflowModel model = new WorkflowModel();
            model.ProcessList = this.service.GetProcessList(0);
            return View(model);
        }
        [HttpPost]
        public JsonResult New(WorkflowModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Workflow workflow = new Workflow();
                    model.Process = this.processService.GetDataById(model.ProcessId);
                    model.SubProcess = this.subProcessService.GetDataById(model.SubProcessId ?? 0);
                    model.Classification = this.classificationService.GetDataById(model.ClassificationId ?? 0);


                    bool ifExists = this.workflowService.CheckDataAndCodeIfExist(model);
                    if (!ifExists)
                    {
                        workflow.Process = model.Process;
                        workflow.SubProcess = model.SubProcess;
                        workflow.Classification = model.Classification;
                        workflow.Code = model.Code;
                        workflow.Name = model.Name;
                        workflow.Description = model.Description;
                        workflow.Active = true;
                        workflow.DateCreated = DateTime.Now;
                        workflow.Requestor = model.RequestorCode != null ? string.Join(",", model.RequestorCode) : string.Empty;
                        workflow.CreatedBy = User.Identity.Name.ToString();
                        workflow.Requestor = string.Join(",", model.RequestorCode);
                        workflow.Id = this.workflowService.Create(workflow);    
                        model.Id = workflow.Id;
                        return Json(new { result = Base.Encrypt(workflow.Id.ToString()), message = MessageCode.saved, code = StatusCode.saved, content=model.Name });
                    }
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed, content = model.Name });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid, content = model.Name });

            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed, content = model.Name });
            }


        }
        #endregion
        #region Manage
        [CryptoValueProvider]
        public ActionResult Manage(long id)
        {
            try
            {
                WorkflowModel model = new WorkflowModel();
                Workflow workflow = this.workflowService.GetDataById(id);
                if (workflow != null)
                {
                    model.Id = workflow.Id;
                    model.Code = workflow.Code;
                    model.Name = workflow.Name;
                    model.Description = workflow.Description;
                    model.Active = workflow.Active;
                    model.ProcessId = workflow.Process.Id;

                    Process process = this.processService.GetDataById(model.ProcessId);
                    model.RoleList = this.service.GetRoleList(workflow.Requestor.Split(new string[] { "," },StringSplitOptions.None), process.SystemCode);

                    model.ProcessList = this.service.GetProcessList(workflow.Process.Id);
                    model.SubProcessId = workflow.SubProcess!=null ? workflow.SubProcess.Id : 0;
                    if (model.SubProcessId != 0)
                    { model.SubProcessList = this.service.GetSubProcessList(workflow.SubProcess.Id); }
                    model.ClassificationId = workflow.Classification!=null ? workflow.Classification.Id : 0;
                    if(model.ClassificationId!=0)
                    {model.ClassificationList = this.service.GetClassificationList(workflow.Classification.Id);}
                }
                if (model != null)
                    return View(model);
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Manage(WorkflowModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Process = this.processService.GetDataById(model.ProcessId);
                    model.SubProcess = this.subProcessService.GetDataById(model.SubProcessId ?? 0);
                    model.Classification = this.classificationService.GetDataById(model.ClassificationId ?? 0);

                    bool ifExists = this.workflowService.CheckDataIfExists(model);
                    if (!ifExists)
                    {
                        Workflow workflow = new Workflow();
                        Process process = new Process();
                        SubProcess subProcess = new SubProcess();
                        Classification classification = new Classification();
                        process = this.processService.GetDataById(model.ProcessId);
                        subProcess = this.subProcessService.GetDataById(model.SubProcessId ?? 0);
                        classification = this.classificationService.GetDataById(model.ClassificationId ?? 0);
                        workflow = this.workflowService.GetDataById(model.Id);
                        workflow.Process = process;
                        workflow.SubProcess = subProcess;
                        workflow.Classification = classification;
                        workflow.Code = model.Code;
                        workflow.Name = model.Name;
                        workflow.Description = model.Description;
                        workflow.Active = model.Active;
                        workflow.Requestor = model.RequestorCode != null ? string.Join(",", model.RequestorCode) : string.Empty;
                        workflow.DateModified = DateTime.Now;
                        workflow.ModifiedBy = User.Identity.Name.ToString();
                        this.workflowService.SaveChanges(workflow);
                        return Json(new { result = Base.Encrypt(workflow.Id.ToString()), message = MessageCode.saved, code = StatusCode.saved, content= MessageCode.saved });

                    }
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed });
            }


        }
        #endregion
        #region Item
        public ActionResult Item(string id, string status)
        {
            try
            {
                long workflowId = Convert.ToInt64(Base.Decrypt(id));
                Workflow workflow = this.workflowService.GetDataById(workflowId);
                if (workflow != null)
                {
                    ViewBag.StatusCode = status;
                    ViewBag.SecuredId = id;
                    ViewBag.Message = MessageCode.GetMessage(status);
                    return View(workflow);
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete
        public JsonResult Delete(long id)
        {
            try
            {
                if (id > 0)
                {
                    this.workflowService.Delete(id);
                    return Json(new { result = StatusCode.done, message = MessageCode.deleted, code = StatusCode.deleted });
                }
            }
            catch (Exception)
            {
            }
            return Json(new { result = StatusCode.failed, message = MessageCode.error });
        }
        #endregion
        #region Filter
        public ActionResult GetSubProcessByProcessId(int selectedValue, int filter)
        {
            List<SelectListItem> subProcessList = new List<SelectListItem>();
            subProcessList = service.GetSubProcessList(selectedValue, filter);
            WorkflowModel model = new WorkflowModel();
            if (subProcessList != null)
            {               
                model.SubProcessList = subProcessList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Partial/SubProcess",model);
                }
            }
            return View(model);
        }

        public ActionResult GetAllRolesByProcessId(int id)
        {
           
            WorkflowModel model = new WorkflowModel();
            Process process = this.processService.GetDataById(id);
            model.RoleList = this.service.GetRoleList(0, process.SystemCode);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Partial/Requestor", model);
            } 
            return View(model);
        }

        public ActionResult GetClassificationBySubProcessId(int selectedValue, int filter)
        {
            List<SelectListItem> classificationList = new List<SelectListItem>();
            classificationList = service.GetClassificationList(selectedValue, filter);
            WorkflowModel model = new WorkflowModel();
            if (classificationList != null)
            {         
                model.ClassificationList = classificationList;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Partial/Classification", model);
                }
            }
            return View(model);
        }
      
        #endregion
        #region Common
        public ActionResult CheckAvailability(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                bool ifExists = this.workflowService.CheckDataIfExists(param);
                if (!ifExists)
                {
                    return Json(new { result = StatusCode.valid, MessageCode.valid, code = StatusCode.valid });
                }
                else
                {
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                }
            }
            return Json(new { result = StatusCode.empty, message = MessageCode.empty, code = StatusCode.empty });
        }
        #endregion

    }
}
