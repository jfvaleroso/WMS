using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Core.Repositories;

namespace WMS.Core.Services.Implementation
{
    public class SubProcessService : ISubProcessService
    {

        private readonly ISubProcessRepository subProcessRepository;
        public SubProcessService(ISubProcessRepository subProcessRepository)
        {
            this.subProcessRepository = subProcessRepository;
        }

        public SubProcess GetDataById(int id)
        {
           return this.subProcessRepository.Get(id);
        }

        public List<SubProcess> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.subProcessRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }

        public List<SubProcess> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.subProcessRepository.GetDataWithPagingAndSearch(searchString, pageNumber, pageSize, out total);
        }

        public void Save(SubProcess entity)
        {
            this.subProcessRepository.Save(entity);
        }

        public int Create(SubProcess entity)
        {
            return this.subProcessRepository.Create(entity);
        }

        public void SaveChanges(SubProcess entity)
        {
            this.subProcessRepository.SaveChanges(entity);
        }

        public void Delete(int id)
        {
            this.subProcessRepository.Delete(id);
        }


        public List<SubProcess> GetDataListByStatus(bool isActive)
        {
            return this.subProcessRepository.Get(x => x.Active.Equals(isActive)).ToList();
        }

        public List<SubProcess> GetDataListWithFilter(bool isActive, int filter)
        {
            return this.subProcessRepository.Get(x => x.Active.Equals(isActive) && x.Process.Id.Equals(filter)).ToList();
        }


        public bool CheckDataIfExists(SubProcess entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", entity.Code);
            parameter.Add("Name", entity.Name);
            parameter.Add("Active", entity.Active);
            parameter.Add("Process", entity.Process);
            List<SubProcess> process = this.subProcessRepository.CheckIfDataExists(parameter);
            if (process.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckDataIfExists(string param)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", param);
            List<SubProcess> process = this.subProcessRepository.CheckIfDataExists(parameter);
            if (process.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckDataAndCodeIfExist(SubProcess entity)
        {
            if (CheckDataIfExists(entity) || CheckDataIfExists(entity.Code))
            {
                return true;
            }
            return false;
        }

       
    }
}
