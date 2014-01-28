using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Core.Repositories;

namespace WMS.Core.Services.Implementation
{
   public class WorkflowMappingService : IWorkflowMappingService
    {
        private readonly IWorkflowMappingRepository workflowMappingRepository;
        public WorkflowMappingService(IWorkflowMappingRepository workflowMappingRepository)
        {
            this.workflowMappingRepository = workflowMappingRepository;
        }

        public WorkflowMapping GetDataById(long id)
        {
            return this.workflowMappingRepository.Get(id);
        }

        public List<WorkflowMapping> GetByExpression(long id)
        {
            return this.workflowMappingRepository.Get(x => x.Workflow.Id.Equals(id)).ToList();
        }

        public List<WorkflowMapping> GetDataListByStatus(bool active)
        {
            return this.workflowMappingRepository.Get(x => x.Active.Equals(active)).ToList();
        }

        public List<WorkflowMapping> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.workflowMappingRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }

        public List<WorkflowMapping> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.workflowMappingRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }


        public void Save(WorkflowMapping entity)
        {
            this.workflowMappingRepository.Save(entity);
        }

        public long Create(WorkflowMapping entity)
        {
            return this.workflowMappingRepository.Create(entity);
        }

        public void SaveChanges(WorkflowMapping entity)
        {
            this.workflowMappingRepository.SaveChanges(entity);
        }

        public void Delete(long id)
        {
            this.workflowMappingRepository.Delete(id);
        }

        public bool CheckDataIfExists(WorkflowMapping entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", entity.Workflow);
            parameter.Add("Approver", entity.Approver);
            parameter.Add("SLA", entity.SLA);
            parameter.Add("AlertTo", entity.AlertTo);
            List<WorkflowMapping> document = this.workflowMappingRepository.CheckIfDataExists(parameter);
            if (document.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckDataIfExists(string param)
        {
           
            return false;
        }



        public bool CheckDataAndCodeIfExist(WorkflowMapping entity)
        {
           
            return false;
        }

        public List<WorkflowMapping> GetDataListWithFilter(bool isActive, int filter)
        {
            return this.workflowMappingRepository.Get(x => x.Active.Equals(isActive)).ToList();
        }

        public List<WorkflowMapping> GetFilteredDataByWorkflow(Workflow workflow)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", workflow);
            List<WorkflowMapping> workflowList = this.workflowMappingRepository.GetFilteredData(parameter);
            return workflowList;
        }
           public void DeleteWorkflowMapping(Workflow workflow)
        {
            List<WorkflowMapping> workflowMappingList = new List<WorkflowMapping>();
            workflowMappingList = GetFilteredDataByWorkflow(workflow);
            if (workflowMappingList.Any())
            {
                foreach (var workflowMapping in workflowMappingList)
                {
                    this.workflowMappingRepository.Delete(workflowMapping);
                }
            }

        }


        #region Web Api
        public List<WorkflowMapping> GetWorkflowMappingByWorfklow(Workflow workflow)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", workflow);
            List<WorkflowMapping> workflowList = this.workflowMappingRepository.GetFilteredData(parameter);
            return workflowList;
        }
        public WorkflowMapping GetCurrentApproverByWorkflowAndLevel(Workflow workflow, int level)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", workflow);
            parameter.Add("LevelId", level);
            WorkflowMapping workflowMapping = this.workflowMappingRepository.GetFilteredData(parameter).FirstOrDefault();
            return workflowMapping;
        }

        #endregion


    }
}

