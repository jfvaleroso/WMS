using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Services.IServices;
using WMS.Helper.Transaction;
using WMS.Core.Helper.Common;
using WMS.Entities;
using WMS.Web.Filter;

namespace WMS.Web.Areas.Setting.Controllers
{
    public class DocumentController : Controller
    {


        #region Constructor
        private readonly IDocumentService documentService;
        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
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
                var documentList = !string.IsNullOrEmpty(searchString) ?
                    this.documentService.GetDataListWithPagingAndSearch(searchString, jtStartIndex, jtPageSize, out total) :
                   this.documentService.GetDataListWithPaging(jtStartIndex, jtPageSize, out total);
                var collection = documentList.Select(x => new
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
        public ActionResult New(Document document)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool ifExists = this.documentService.CheckDataAndCodeIfExist(document);
                    if (!ifExists)
                    {
                        document.Code = document.Code.ToUpper();
                        document.Active = true;
                        document.DateCreated = DateTime.Now;
                        document.CreatedBy = User.Identity.Name.ToString();
                        document.Id = this.documentService.Create(document);
                        return Json(new { result = Base.Encrypt(document.Id.ToString()), message = MessageCode.saved, code = StatusCode.saved, content = document.Name.ToString() });
                       
                    }
                    return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                }
                return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
            }
            catch (Exception ex)
            {
                return Json(new { result = StatusCode.failed, message = ex.Message.ToString(), code = StatusCode.failed, content = document.Name.ToString() });
            }
        }
        #endregion
        #region Manage
        [CryptoValueProvider]
        public ActionResult Manage(int id)
        {
            try
            {
                Document document = this.documentService.GetDataById(id);
                if (document != null)
                    return View(document);
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Manage(Document model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool ifExists = this.documentService.CheckDataIfExists(model);
                    if (!ifExists)
                    {

                        Document document = new Document();
                        document = this.documentService.GetDataById(model.Id);
                        document.Code = model.Code.ToUpper();
                        document.Name = model.Name;
                        document.Description = model.Description;
                        document.Active = model.Active;
                        document.DateModified = DateTime.Now;
                        document.ModifiedBy = User.Identity.Name.ToString();
                        this.documentService.SaveChanges(document);
                        return Json(new { result = Base.Encrypt(document.Id.ToString()), message = MessageCode.modified, code = StatusCode.modified, content= document.Name });
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
                int documentId = Convert.ToInt32(Base.Decrypt(id));
                Document document = this.documentService.GetDataById(documentId);
                if (document != null)
                {
                    ViewBag.StatusCode = status;
                    ViewBag.SecuredId = id;
                    ViewBag.Message = MessageCode.GetMessage(status);
                    return View(document);
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
                    this.documentService.Delete(id);
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
                bool ifExists = this.documentService.CheckDataIfExists(param);
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
