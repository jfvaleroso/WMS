using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Core.Repositories;

namespace WMS.Core.Services.Implementation
{
    public class WorkflowService: IWorkflowService
    {
        private readonly IWorkflowRepository workflowRepository;
        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            this.workflowRepository = workflowRepository;
        }
        public Workflow GetDataById(long id)
        {
            return this.workflowRepository.Get(id);
        }
        public List<Workflow> GetDataListByStatus(bool active)
        {
            return this.workflowRepository.Get(x => x.Active.Equals(active)).ToList();
        }
        
        public List<Workflow> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.workflowRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }
        public List<Workflow> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.workflowRepository.GetDataWithPagingAndSearch(searchString, pageNumber, pageSize, out total);
        }
        public void Save(Workflow entity)
        {
            this.workflowRepository.Save(entity);
        }
        public long Create(Workflow entity)
        {
            return this.workflowRepository.Create(entity);
        }
        public void SaveChanges(Workflow entity)
        {
            this.workflowRepository.SaveChanges(entity);
        }
        public void Delete(long id)
        {
            this.workflowRepository.Delete(id);
        }
        public bool CheckDataIfExists(Workflow entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            //parameter.Add("Process", entity.Process);
            //parameter.Add("SubProcess", entity.SubProcess);
            //parameter.Add("Classification", entity.Classification);
            parameter.Add("Code", entity.Code);
            parameter.Add("Name", entity.Name);
            parameter.Add("Active", entity.Active);
            List<Workflow> workflow = this.workflowRepository.CheckIfDataExists(parameter);
            if (workflow.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckDataIfExists(string param)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", param);
            List<Workflow> document = this.workflowRepository.CheckIfDataExists(parameter);
            if (document.Count() > 0)
            {
                return true;
            }
            return false;
        }


        public bool CheckDataAndCodeIfExist(Workflow entity)
        {
            if (CheckDataIfExists(entity) || CheckDataIfExists(entity.Code))
            {
                return true;
            }
            return false;
        }



        public List<Workflow> GetDataListWithFilter(bool isActive, long filter)
        {
            return this.workflowRepository.Get(x => x.Active.Equals(isActive) && x.Id.Equals(filter)).ToList();
        }

        public Workflow GetWorkflowByCode(string code)
        {
            return this.workflowRepository.Get(x => x.Code.Equals(code)).FirstOrDefault();
        }

    }
}
