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

namespace WMS.Web.Areas.Setting.Controllers
{
    public class StatusController : Controller
    {
        #region Constructor
        private readonly IStatusService statusService;
        public StatusController(IStatusService statusService)
        {
            this.statusService = statusService;
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
                var dataList = !string.IsNullOrEmpty(searchString) ?
                    this.statusService.GetDataListWithPagingAndSearch(searchString, jtStartIndex, jtPageSize, out total) :
                   this.statusService.GetDataListWithPaging(jtStartIndex, jtPageSize, out total);
                var collection = dataList.Select(x => new
                {
                    Id = x.Id,
                    SecuredId = Base.Encrypt(x.Id.ToString()),
                    Active = x.Active,
                    Code = x.Code,
                    Name = x.Name,
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
            return View();
        }
       
        [HttpPost]
        public ActionResult New(Status status)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    bool  dataExists = this.statusService.CheckDataAndCodeIfExist(status);
                    if (!dataExists)
                    {
                        status.Code = status.Code.ToUpper();
                        status.Active = true;
                        status.DateCreated = DateTime.Now;
                        status.CreatedBy = User.Identity.Name.ToString();
                        status.Id = this.statusService.Create(status);
                        return Json(new { result = Base.Encrypt(status.Id.ToString()), message = MessageCode.saved, code = StatusCode.saved, content = status.Name.ToString() });
                       
                    }
                    ModelState.AddModelError(StatusCode.existed, MessageCode.existed);
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed, content = status.Name.ToString() });
            }
        }
        #endregion
        #region Manage
        [CryptoValueProvider]
        public ActionResult Manage(int id)
        {
            try
            {
                Status status = this.statusService.GetDataById(id);
                if (status != null)
                    return View(status);
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Manage(Status model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool ifExists = this.statusService.CheckDataIfExists(model);
                    if (!ifExists)
                    {

                        Status status = new Status();
                        status = this.statusService.GetDataById(model.Id);
                        status.Name = model.Name;
                        status.Description = model.Description;
                        status.Active = model.Active;
                        status.DateModified = DateTime.Now;
                        status.ModifiedBy = User.Identity.Name.ToString();
                        this.statusService.SaveChanges(status);
                        return Json(new { result = Base.Encrypt(status.Id.ToString()), message = MessageCode.modified, code = StatusCode.modified, content = status.Name });
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
                int statusId = Convert.ToInt32(Base.Decrypt(id));
                Status stat = this.statusService.GetDataById(statusId);
                if (stat != null)
                {
                    ViewBag.StatusCode = status;
                    ViewBag.SecuredId = id;
                    ViewBag.Message = MessageCode.GetMessage(status);
                    return View(stat);
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
                    this.statusService.Delete(id);
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
                bool ifExists = this.statusService.CheckDataIfExists(param);
                if (!ifExists)
                {
                   
                    return Json(new { result = StatusCode.valid,MessageCode.valid, code = StatusCode.valid });
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
