using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Core.Helper;
using WMS.Web.Filter;
using WMS.Helper;
using WMS.Helper.Transaction;
using WMS.Core.Helper.Common;
using WMS.Webservice.IServices;
using WMS.Web.Helper;
using WMS.Web.Areas.Setting.Models;

namespace WMS.Web.Areas.Setting.Controllers
{
    public class ProcessController : Controller
    {
        #region Constructor
        private readonly IProcessService processService;
        private readonly ICommonService commonService;
        private readonly Service service;
        public ProcessController(IProcessService processService, ICommonService commonService)
        {
            this.processService = processService;
            this.commonService = commonService;
            this.service = new Service(commonService);
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
                var processList = !string.IsNullOrEmpty(searchString) ?
                    this.processService.GetDataListWithPagingAndSearch(searchString, jtStartIndex, jtPageSize, out total) :
                   this.processService.GetDataListWithPaging(jtStartIndex, jtPageSize, out total);
                var collection = processList.Select(x => new
                {
                    Id = x.Id,
                    SecuredId = Base.Encrypt(x.Id.ToString()),
                    Active = x.Active,
                    ProcessCode = x.Code,
                    ProcessName = x.Name,
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
            ProcessModel model = new ProcessModel();
            model.SystemList = this.service.GetAllSystems(string.Empty);
            return View(model);
        }
        [HttpPost]
        public JsonResult New(ProcessModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Process process = new Process();
                    bool ifExists = this.processService.CheckDataAndCodeIfExist(model);
                    if (!ifExists)
                    {
                        process.SystemCode = model.SystemCode;
                        process.Code = Base.GenerateCode(model.SystemCode, model.Code).ToUpper();
                        process.Name = model.Name;
                        process.Description = model.Description;
                        process.Active = true;
                        process.DateCreated = DateTime.Now;
                        process.CreatedBy = User.Identity.Name.ToString();
                        process.Id = this.processService.Create(process);
                        model.Id = process.Id;
                        return Json(new { result = Base.Encrypt(process.Id.ToString()), message = MessageCode.saved, code = StatusCode.saved, content= model.Name });
                    }
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed, content = model.Name.ToString() });
            }
        }
        #endregion
        #region Manage
        [CryptoValueProvider]
        public ActionResult Manage(int id)
        {
            try
            {
                Process process = this.processService.GetDataById(id);
                ProcessModel model = new ProcessModel();
                model.SystemList = this.service.GetAllSystems(process.SystemCode);
                model.SystemCode = process.SystemCode;
                model.Code = process.Code;
                model.Name = process.Name;
                model.Description = process.Description;
                model.DateCreated = process.DateCreated;
                model.CreatedBy = process.CreatedBy;
                model.Id = process.Id;
                model.Active = process.Active;
                if (model != null)
                    return View(model);
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Manage(ProcessModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool ifExists = this.processService.CheckDataIfExists(model);
                    if (!ifExists)
                    {

                        Process process = new Process();
                        process = this.processService.GetDataById(model.Id);
                        process.SystemCode = model.SystemCode.ToUpper();
                        process.Code = Base.GenerateCode(model.SystemCode, model.Code).ToUpper();
                        process.Name = model.Name;
                        process.Description = model.Description;
                        process.Active = model.Active;
                        process.DateModified = DateTime.Now;
                        process.ModifiedBy = User.Identity.Name.ToString();
                        this.processService.SaveChanges(process);
                        return Json(new { result = Base.Encrypt(process.Id.ToString()), message = MessageCode.modified, code = StatusCode.modified, content = process.Name });
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
        #region Item
        public ActionResult Item(string id, string status)
        {
            try
            {
                int processId = Convert.ToInt32(Base.Decrypt(id));
                Process process = this.processService.GetDataById(processId);
                if (process != null)
                {
                    ViewBag.StatusCode = status;
                    ViewBag.SecuredId = id;
                    ViewBag.Message = MessageCode.GetMessage(status);
                    return View(process);
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
                    this.processService.Delete(id);
                    return Json(new { result = StatusCode.done, message = MessageCode.deleted, code= StatusCode.deleted });
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
                bool ifExists = this.processService.CheckDataIfExists(param);
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
