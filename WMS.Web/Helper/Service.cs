using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Core.Services.IServices;
using System.Web.Mvc;
using WMS.Webservice.IServices;

namespace WMS.Web.Helper
{
    public class Service
    {
        private readonly IProcessService processService;
        private readonly ISubProcessService subProcessService;
        private readonly IClassificationService classificationService;
        private readonly IWorkflowService workflowService;
        private readonly IDocumentService documentService;
        private readonly IRoleService roleService;
        private readonly ICommonService commonService;
        private readonly IStatusService statusService;

        #region Constructors
        public Service(IProcessService processService)
        {
            this.processService = processService;        
        }
        public Service(IWorkflowService workflowService, IRoleService roleService)
        {
            this.workflowService = workflowService;
            this.roleService = roleService;
        }
        public Service(ISubProcessService subProcessService)
        {
            this.subProcessService = subProcessService;
        }
        public Service(IProcessService processService, ISubProcessService subProcessService)
        {
            this.processService = processService;
            this.subProcessService = subProcessService;
        }
        public Service(IProcessService processService, ISubProcessService subProcessService, IClassificationService classificationService, IRoleService roleService)
        {
            this.subProcessService = subProcessService;
            this.processService = processService;
            this.classificationService = classificationService;
            this.roleService = roleService;
        }
        public Service(IDocumentService documentService, IWorkflowService workflowService)
        {
            this.documentService = documentService;
            this.workflowService = workflowService;
        }
        public Service(IStatusService statusService, IWorkflowService workflowService)
        {
            this.statusService = statusService;
            this.workflowService = workflowService;
        }
        public Service(ICommonService commonService)
        {
            this.commonService = commonService;
        }
        #endregion

        #region SelectedListITem
        public List<SelectListItem> GetProcessList(int selectedValue)
        {

            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.processService.GetDataListByStatus(true);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text =string.Format("{0}-{1}", item.Code.ToString(),item.Name.ToString());
                  //sourceItem.Selected = item.Id.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetSubProcessList(int selectedValue)
        {

            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.subProcessService.GetDataListByStatus(true);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text = string.Format("{0}-{1}", item.Code.ToString(), item.Name.ToString());
                    sourceItem.Selected = item.Id.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetClassificationList(int selectedValue)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.classificationService.GetDataListByStatus(true);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text = string.Format("{0}-{1}", item.Code.ToString(), item.Name.ToString());
                    sourceItem.Selected = item.Id.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetSubProcessList(int selectedValue, int filter)
        {

            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.subProcessService.GetDataListWithFilter(true, filter);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text = string.Format("{0}-{1}", item.Code.ToString(), item.Name.ToString());
                    sourceItem.Selected = item.Id.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }

            }
            return source;
        }
        public List<SelectListItem> GetClassificationList(int selectedValue, int filter)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.classificationService.GetDataListWithFilter(true, filter);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text = string.Format("{0}-{1}", item.Code.ToString(), item.Name.ToString());
                    sourceItem.Selected = item.Id.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetWorkflowList(long selectedValue, long filter)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.workflowService.GetDataListWithFilter(true, filter);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text = string.Format("{0}-{1}", item.Code.ToString(), item.Name.ToString());
                    sourceItem.Selected = item.Id.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetStatusList(bool isActive)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.statusService.GetDataListByStatus(isActive);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text = string.Format("{0}-{1}", item.Code.ToString(), item.Name.ToString());
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetDocumentList(int selectedValue, int filter)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.documentService.GetDataListWithFilter(true, filter);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.Id.ToString();
                    sourceItem.Text = string.Format("{0}-{1}", item.Code.ToString(), item.Name.ToString());
                    sourceItem.Selected = item.Id.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetRoleList(int selectedValue,string systemCode)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.roleService.GetAllRoles(systemCode);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.RoleCode.ToString();
                    sourceItem.Text = string.Format("{0} ({1})", item.RoleCode.ToString(), item.RoleName.ToString());
                    sourceItem.Selected = item.RoleCode.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }

        public List<SelectListItem> GetRoleList(string[] selectedValue, string systemCode)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.roleService.GetAllRoles(systemCode);
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.RoleCode.ToString();
                    sourceItem.Text = string.Format("{0} ({1})", item.RoleCode.ToString(), item.RoleName.ToString());
                    sourceItem.Selected = selectedValue.Contains(item.RoleCode) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        public List<SelectListItem> GetOperatorList(params string[] operators)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            foreach (var item in operators)
	        {
		            SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.ToString();
                    sourceItem.Text =  item.ToString();
                    source.Add(sourceItem);
	        }
            return source;
        }
        public List<SelectListItem> GetAllSystems(string selectedValue)
        {
            List<SelectListItem> source = new List<SelectListItem>();
            var items = this.commonService.GetAllSystems();
            if (items != null)
            {
                foreach (var item in items)
                {
                    SelectListItem sourceItem = new SelectListItem();
                    sourceItem.Value = item.SystemCode.ToString();
                    sourceItem.Text = string.Format("{0} ({1})", item.SystemCode.ToString(), item.SystemName.ToString());
                    sourceItem.Selected = item.SystemCode.Equals(selectedValue) ? true : false;
                    source.Add(sourceItem);
                }
            }
            return source;
        }
        #endregion

    }
}