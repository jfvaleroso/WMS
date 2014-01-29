using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Core.Services.IServices;
using WMS.Web.Helper;
using WMS.Web.Models;
using WMS.Web.Filter;
using WMS.Entities;
using WMS.Helper.Transaction;
using WMS.Core.Helper.Common;

namespace WMS.Web.Controllers
{
    public class DocumentMappingController : Controller
    {
        #region Constructor
        private readonly IWorkflowService workflowService;
        private readonly IDocumentService documentService;
        private readonly IDocumentMappingService documentMappingService;
        private readonly Service service;
        public DocumentMappingController(IWorkflowService workflowService, IDocumentService documentService, IDocumentMappingService documentMappingService)
        {
            this.workflowService = workflowService;
            this.documentService = documentService;
            this.documentMappingService = documentMappingService;
            this.service = new Service(this.documentService, this.workflowService);
        }
        #endregion



        #region Index
        [CryptoValueProvider]
        public ActionResult Index(long id)
        {
            if(id!=0)
            {
                try
                {
                    DocumentMappingModel model = new DocumentMappingModel();
                    model.WorkflowId = id;
                    model.WorkflowList = this.service.GetWorkflowList(id,id);
                    model.DocumentList = this.service.GetDocumentList(0, 0);
                    model.SecuredId= Base.Encrypt(id.ToString());
                    model.DocumentMappingList = GetCurrentDocumentMapping() ?? null;
                    if (model != null)
                        return View(model);
                }
                catch (Exception)
                {

                }
             }
            return RedirectToAction("Index");
        }
        #endregion
        #region Save
        [HttpPost]
        public ActionResult SaveDocumentMapping(DocumentMappingModel model)
        {
            try
            {
                List<DocumentMapping> currentDocumentMapping = GetCurrentDocumentMapping();
                if (currentDocumentMapping.Any())
                {
                    foreach (var document in currentDocumentMapping)
                    {
                        bool ifExists = this.documentMappingService.CheckDataIfExists(document);
                        if (!ifExists)
                        {
                            document.CreatedBy = User.Identity.Name;
                            document.DateCreated = DateTime.Now;
                            document.Active = true;
                            this.documentMappingService.Save(document);
                        }
                    }
                    ClearSession();
                    return Json(new { result = Base.Encrypt(model.WorkflowId.ToString()), message = MessageCode.saved, code = StatusCode.saved, content = MessageCode.saved });
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
        public ActionResult AddDocument(DocumentMappingModel model)
        {
            if (ModelState.IsValid)
            {
                DocumentMapping documentMapping = new DocumentMapping();
                List<DocumentMapping> currentDocumentMapping = GetCurrentDocumentMapping();
                documentMapping.Mandatory = model.Mandatory;
                documentMapping.Document = this.documentService.GetDataById(model.DocumentId);
                documentMapping.Workflow = this.workflowService.GetDataById(model.WorkflowId);
                if (currentDocumentMapping.Any())
                {
                    if (!currentDocumentMapping.Exists(x => x.Document.Id.Equals(documentMapping.Document.Id)))
                    {
                        model.DocumentMappingList.Add(documentMapping);
                        currentDocumentMapping.Add(documentMapping);

                    }
                    else
                    {
                        ModelState.AddModelError("StatusError", "already added");
                        return Json(new { result = StatusCode.existed, message = MessageCode.existed, code = StatusCode.existed });
                    }

                }
                else
                {
                    currentDocumentMapping.Add(documentMapping);

                }
                model.DocumentMappingList = currentDocumentMapping;
                if (Request.IsAjaxRequest())
                {
                   return PartialView("Partial/Document", model);
                }
                return Json(new { result = StatusCode.saved, message = MessageCode.saved, code = StatusCode.saved });
            }
            return Json(new { result = StatusCode.failed, message = MessageCode.error, code = StatusCode.invalid });
        }
        public ActionResult RemoveDocument(int id)
        {

            DocumentMappingModel model = new DocumentMappingModel();
            List<DocumentMapping> currentDocumentMapping = RemoveItemFromCurrentDocumentMapping(id);
            model.DocumentMappingList = currentDocumentMapping;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Partial/Document", model);
            }
            return View(model);
        }
        #endregion
        #region private method
        private List<DocumentMapping> GetCurrentDocumentMapping()
        {
            var documentMapping = (List<DocumentMapping>)Session["documentMapping"];
            if (documentMapping == null)
            {
                Session["documentMapping"] = documentMapping = new List<DocumentMapping>();
            }
            return documentMapping;
        }
        private List<DocumentMapping> GetCurrentDocumentMapping(long id)
        {   
            Workflow workflow = this.workflowService.GetDataById(id);
            List<DocumentMapping> documentMappingList = this.documentMappingService.GetFilteredDataByWorkflow(workflow);
            var documentMapping = (List<DocumentMapping>)Session["documentMapping"];
            if (documentMappingList != null)
            {
                Session["documentMapping"] = documentMapping = documentMappingList;
            }
            return documentMapping;
        }
        private List<DocumentMapping> RemoveItemFromCurrentDocumentMapping(int id)
        {
            List<DocumentMapping> currentDocumentMapping = GetCurrentDocumentMapping();
            currentDocumentMapping.RemoveAll(x => x.Document.Id.Equals(id));
            return currentDocumentMapping;
        }
        private DocumentMapping GetItemFromCurrentDocumentMapping(string code)
        {
            List<DocumentMapping> currentDocumentMapping = GetCurrentDocumentMapping();
            DocumentMapping documentMapping= currentDocumentMapping.Find(x => x.Document.Code.Equals(code));
            return documentMapping;
        }
        private void PopulateCurrentDocumentMapping(List<DocumentMapping> documentMappingList)
        {
            Session["documentMapping"] = documentMappingList;
        }
        private void ClearSession()
        {
            Session["documentMapping"] = null;
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
                    DocumentMappingModel model = new DocumentMappingModel();
                    model.WorkflowId = id;
                    model.WorkflowList = this.service.GetWorkflowList(id, id);
                    model.DocumentList = this.service.GetDocumentList(0, 0);
                    model.SecuredId = Base.Encrypt(id.ToString());
                    model.DocumentMappingList = GetCurrentDocumentMapping(id) ??  GetCurrentDocumentMapping();
                    return View(model);  
                }
                catch (Exception)
                {

                }
            }
            return RedirectToAction("Index", "Workflow");
        }
        public ActionResult SaveChangesInDocumentMapping(DocumentMappingModel model)
        {
            try
            {
                List<DocumentMapping> currentDocumentMapping = GetCurrentDocumentMapping();
                if (currentDocumentMapping.Any())
                {
                    //remove previous documentmapping
                    Workflow workflow= this.workflowService.GetDataById(model.WorkflowId);
                    this.documentMappingService.DeleteDocumentMapping(workflow);

                    //add new document mapping
                    foreach (var document in currentDocumentMapping)
                    {
                        bool ifExists = this.documentMappingService.CheckDataIfExists(document);
                        if (!ifExists)
                        {
                            document.CreatedBy = User.Identity.Name;
                            document.DateCreated = DateTime.Now;
                            document.Active = true;
                            this.documentMappingService.Save(document);
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

        public ActionResult Modify(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                DocumentMappingModel model = new DocumentMappingModel();
                DocumentMapping currentDocumentMapping = GetItemFromCurrentDocumentMapping(code);
                model.Document = currentDocumentMapping.Document;
                model.Active = currentDocumentMapping.Active;
                model.Mandatory = currentDocumentMapping.Mandatory;
                return View(model);
            }
            return View();
        }
    }
}
