using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Repositories;
using WMS.Core.Services.IServices;
using WMS.Entities;

namespace WMS.Core.Services.Implementation
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository statuRepository;
        public StatusService(IStatusRepository statuRepository)
        {
            this.statuRepository = statuRepository;
        }
        public Status GetDataById(int id)
        {
            return this.statuRepository.Get(id);
        }

        public List<Status> GetDataListByStatus(bool active)
        {
            return this.statuRepository.Get(x => x.Active.Equals(active)).ToList();
        }

        public List<Status> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.statuRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }

        public List<Status> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.statuRepository.GetDataWithPagingAndSearch(searchString, pageNumber, pageSize, out total);
        }

        public void Save(Status entity)
        {
            this.statuRepository.Save(entity);
        }

        public int Create(Status entity)
        {
            return this.statuRepository.Create(entity);
        }

        public void SaveChanges(Status entity)
        {
            this.statuRepository.SaveChanges(entity);
        }

        public void Delete(int id)
        {
            this.statuRepository.Delete(id);
        }

        public bool CheckDataIfExists(Status entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Name", entity.Name);
            parameter.Add("Code", entity.Code);
            parameter.Add("Active", entity.Active);
            List<Status> document = this.statuRepository.CheckIfDataExists(parameter);
            if (document.Count() > 0)
            {
                return true;
            }
            return false;
        }


        public bool CheckDataIfExists(string param)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", param);
            List<Status> document = this.statuRepository.CheckIfDataExists(parameter);
            if (document.Count() > 0)
            {
                return true;
            }
            return false;
        }


        public bool CheckDataAndCodeIfExist(Status entity)
        {
            if (CheckDataIfExists(entity) || CheckDataIfExists(entity.Code))
            {
                return true;
            }
            return false;
        }

        public List<Status> GetDataListWithFilter(bool isActive, int filter)
        {
            return this.statuRepository.Get(x => x.Active.Equals(isActive)).ToList();
        }
    }
}

