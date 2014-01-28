using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Services.IServices;
using WMS.Web.Helper;
using WMS.Web.Areas.Setting.Models;
using WMS.Entities;
using WMS.Web.Filter;
using WMS.Core.Helper.Common;
using WMS.Helper.Transaction;

namespace WMS.Web.Areas.Setting.Controllers
{
    public class ClassificationController : Controller
    {

        #region Constructor
        private readonly IClassificationService classificationService;
        private readonly ISubProcessService subProcessService;
        private readonly IProcessService processService;
        private readonly Service service;
        public ClassificationController(IClassificationService classificationService, ISubProcessService subProcessService, IProcessService processService)
        {
            this.classificationService = classificationService;
            this.subProcessService = subProcessService;
            this.processService = processService;
            this.service = new Service(this.subProcessService);
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
                var classificationList = !string.IsNullOrEmpty(searchString) ?
                    this.classificationService.GetDataListWithPagingAndSearch(searchString, jtStartIndex, jtPageSize, out total) :
                   this.classificationService.GetDataListWithPaging(jtStartIndex, jtPageSize, out total);
                var collection = classificationList.Select(x => new
                {
                    Id = x.Id,
                    SecuredId = Base.Encrypt(x.Id.ToString()),
                    Active = x.Active,
                    SubProcessName = x.SubProcess != null ? x.SubProcess.Name : string.Empty,
                    ClassificationCode = x.Code,
                    ClassificationName = x.Name,
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
            ClassificationModel model = new ClassificationModel();
            model.SubProcessList = this.service.GetSubProcessList(0);
            return View(model);
        }
        [HttpPost]
        public JsonResult New(ClassificationModel model)
        {
            try
            {
                Classification classification = new Classification();
                model.SubProcess = this.subProcessService.GetDataById(model.SubProcessId);
                if (ModelState.IsValid)
                {
                    bool ifExists = this.classificationService.CheckDataAndCodeIfExist(model);
                    if (!ifExists)
                    {
                        classification.SubProcess = model.SubProcess;
                        classification.Code = model.Code.ToUpper();
                        classification.Name = model.Name;
                        classification.Description = model.Description;
                        classification.Active = true;
                        classification.DateCreated = DateTime.Now;
                        classification.CreatedBy = User.Identity.Name.ToString();
                        classification.Id = this.classificationService.Create(classification);
                        model.Id = classification.Id;
                        return Json(new { result = Base.Encrypt(classification.Id.ToString()), message = MessageCode.saved, code = StatusCode.saved, content=model.Name });
                    }
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });

            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed, content=model.Name });
            }


        }
        #endregion
        #region Manage
        [CryptoValueProvider]
        public ActionResult Manage(int id)
        {
            try
            {
                ClassificationModel model = new ClassificationModel();
                Classification classification = this.classificationService.GetDataById(id);
                
                model.Id = classification.Id;
                model.Active = classification.Active;
                model.Code = classification.Code;
                model.Name = classification.Name;
                model.Description = classification.Description;
                model.DateCreated = classification.DateCreated;
                model.CreatedBy = classification.CreatedBy;
                model.SubProcessId = classification.SubProcess.Id;
                model.SubProcessList = this.service.GetSubProcessList(classification.SubProcess.Id);
                if (model != null)
                    return View(model);
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Manage(ClassificationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.SubProcess = this.subProcessService.GetDataById(model.SubProcessId);
                    bool ifExists = this.classificationService.CheckDataIfExists(model);
                    if (!ifExists)
                    {
                        Classification classification = new Classification();
                        SubProcess subProcess = new SubProcess();
                        subProcess = this.subProcessService.GetDataById(model.SubProcessId);
                        classification = this.classificationService.GetDataById(model.Id);
                        classification.SubProcess = subProcess;
                        classification.Code = model.Code;
                        classification.Name = model.Name;
                        classification.Description = model.Description;
                        classification.Active = model.Active;
                        classification.DateModified = DateTime.Now;
                        classification.ModifiedBy = User.Identity.Name.ToString();
                        this.classificationService.SaveChanges(classification);
                        return Json(new { result = Base.Encrypt(classification.Id.ToString()), message = MessageCode.modified, code = StatusCode.modified, content= model.Name });
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
                int classificationId = Convert.ToInt32(Base.Decrypt(id));
                Classification classification = this.classificationService.GetDataById(classificationId);
                if (classification != null)
                {
                    ViewBag.StatusCode = status;
                    ViewBag.SecuredId = id;
                    ViewBag.Message = MessageCode.GetMessage(status);
                    return View(classification);
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
                    this.classificationService.Delete(id);
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
                bool ifExists = this.classificationService.CheckDataIfExists(param);
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
