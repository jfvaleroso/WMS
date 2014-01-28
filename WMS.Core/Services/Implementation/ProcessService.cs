using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Services.IServices;
using WMS.Core.Repositories;
using WMS.Entities;
using WMS.Core.UnitofWork;
using System.Linq.Expressions;



namespace WMS.Core.Services.Implementation
{
    public class ProcessService : IProcessService
    {
        private readonly IProcessRepository processRepository;
        public ProcessService(IProcessRepository processRepository)
        {
            this.processRepository = processRepository;
        }

        public Process GetDataById(int id)
        {
            return this.processRepository.Get(id);
        }
        public List<Process> GetDataListByStatus(bool active)
        {
            return this.processRepository.Get(x => x.Active.Equals(active)).ToList();
        }
        public List<Process> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.processRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }
        public List<Process> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.processRepository.GetDataWithPagingAndSearch(searchString, pageNumber, pageSize, out total);
        }
        public void Save(Process entity)
        {
            this.processRepository.Save(entity);
        }
        public int Create(Process entity)
        {
           return this.processRepository.Create(entity);
        }
        public void SaveChanges(Process entity)
        {
            this.processRepository.SaveChanges(entity);
        }
        public void Delete(int id)
        {
            this.processRepository.Delete(id);
        }
        public bool CheckDataIfExists(Process entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", entity.Code);
            parameter.Add("SystemCode", entity.SystemCode);
            parameter.Add("Name", entity.Name);
            parameter.Add("Active", entity.Active);
            parameter.Add("Description", entity.Description);
            List<Process> process = this.processRepository.CheckIfDataExists(parameter);
            if (process.Count() > 0 )
            {
                return true;
            }
            return false;
        }
        public bool CheckDataIfExists(string param)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", param);
            List<Process> process = this.processRepository.CheckIfDataExists(parameter);
            if (process.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckDataAndCodeIfExist(Process entity)
        {
            if (CheckDataIfExists(entity) || CheckDataIfExists(entity.Code))
            {
                return true;
            }
            return false;
        }

       
    }
}
