using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Web.Areas.Setting.Models;
using WMS.Core.Helper;
using WMS.Web.Filter;
using WMS.Core.Helper.Common;
using WMS.Web.Helper;
using WMS.Helper.Transaction;

namespace WMS.Web.Areas.Setting.Controllers
{
    public class SubProcessController : Controller
    {
        #region Constructor
        private readonly ISubProcessService subProcessService;
        private readonly IProcessService processService;
        private readonly Service service;
        public SubProcessController(ISubProcessService subProcessService, IProcessService processService)
        {
            this.subProcessService = subProcessService;
            this.processService = processService;
            this.service = new Service(this.processService,this.subProcessService);
        }
        #endregion
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult DataListWithPaging(string searchString = "", int jtStartIndex = 1, int jtPageSize = 15)
        {
            try
            {
                long total = 0;
                var subProcessList = !string.IsNullOrEmpty(searchString) ?
                   this.subProcessService.GetDataListWithPagingAndSearch(searchString, jtStartIndex, jtPageSize, out total) :
                   this.subProcessService.GetDataListWithPaging(jtStartIndex, jtPageSize, out total);
                var collection = subProcessList.Select(x => new
                {
                    Id = x.Id,
                    SecuredId = Base.Encrypt(x.Id.ToString()),
                    Active = x.Active,
                    ProcessName = x.Process!=null ?  x.Process.Name : string.Empty,
                    SubProcessCode = x.Code,
                    SubProcessName = x.Name,
                    Description = x.Description
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
            SubProcessModel entity = new SubProcessModel();
            entity.ProcessList = this.service.GetProcessList(0);
            return View(entity);
        }
        [HttpPost]
        public JsonResult New(SubProcessModel model)
        {
            try
            {
                SubProcess subProcess = new SubProcess();
                model.Process = this.processService.GetDataById(model.ProcessId);
                if (ModelState.IsValid)
                {
                    bool ifExists = this.subProcessService.CheckDataAndCodeIfExist(model);
                    if (!ifExists)
                    {
                        subProcess.Process = model.Process;
                        subProcess.Code = model.Code.ToUpper();
                        subProcess.Name = model.Name;
                        subProcess.Description = model.Description;
                        subProcess.Active = true;
                        subProcess.DateCreated = DateTime.Now;
                        subProcess.CreatedBy = User.Identity.Name.ToString();
                        subProcess.Id = this.subProcessService.Create(subProcess);
                        model.Id = subProcess.Id;
                        return Json(new { result = Base.Encrypt(subProcess.Id.ToString()), message = MessageCode.saved, code = StatusCode.saved, content= subProcess.Name });
                    }
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });

            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed, content = model.Name });
            }


        }
        #endregion
        #region Manage
        [CryptoValueProvider]
        public ActionResult Manage(int id)
        {
            try
            {
                SubProcessModel model = new SubProcessModel();
                SubProcess subProcess = this.subProcessService.GetDataById(id);
                model.Id = subProcess.Id;
                model.Active = subProcess.Active;
                model.Code = subProcess.Code;
                model.Name = subProcess.Name;
                model.Active = subProcess.Active;
                model.Description = subProcess.Description;
                model.ProcessId = subProcess.Process.Id;
                model.CreatedBy = subProcess.CreatedBy;
                model.DateCreated = subProcess.DateCreated;
                model.ProcessList = this.service.GetProcessList(subProcess.Process.Id);
                if (model != null)
                    return View(model);
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Manage(SubProcessModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    bool ifExists = this.subProcessService.CheckDataIfExists(model);
                    if (!ifExists)
                    {
                        SubProcess subProcess = new SubProcess();
                        Process process = new Process();
                        process = this.processService.GetDataById(model.ProcessId);
                        subProcess = this.subProcessService.GetDataById(model.Id);
                        subProcess.Id = model.Id;
                        subProcess.Process = process;
                        subProcess.Code = model.Code;
                        subProcess.Name = model.Name;
                        subProcess.Description = model.Description;
                        subProcess.Active = model.Active;
                        subProcess.DateModified = DateTime.Now;
                        subProcess.ModifiedBy = User.Identity.Name.ToString();
                        this.subProcessService.SaveChanges(subProcess);
                        return Json(new { result = Base.Encrypt(subProcess.Id.ToString()), message = MessageCode.modified, code = StatusCode.modified, content = subProcess.Name });
                    }
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed, content = model.Name });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed, content = model.Name });
            }


        }
        #endregion
        #region Item
        public ActionResult Item(string id, string status)
        {
            try
            {
                int subProcessId = Convert.ToInt32(Base.Decrypt(id));
                SubProcess subProcess = this.subProcessService.GetDataById(subProcessId);
                if (subProcess != null)
                {
                    ViewBag.StatusCode = status;
                    ViewBag.SecuredId = id;
                    ViewBag.Message = MessageCode.GetMessage(status);
                    return View(subProcess);
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
        public JsonResult Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    this.subProcessService.Delete(id);
                    return Json(new { result = StatusCode.done, message = MessageCode.deleted, code = StatusCode.deleted });
                }
            }
            catch (Exception)
            {
            }
            return Json(new { result = StatusCode.failed, message = MessageCode.error });
        }
        #endregion
        #region Common
        public ActionResult CheckAvailability(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                bool ifExists = this.subProcessService.CheckDataIfExists(param);
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
