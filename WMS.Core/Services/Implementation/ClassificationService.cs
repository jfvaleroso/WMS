using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Core.Repositories;

namespace WMS.Core.Services.Implementation
{
    public class ClassificationService : IClassificationService
    {
        private readonly IClassificationRepository classificationRepository;
        public ClassificationService(IClassificationRepository classificationRepository)
        {
            this.classificationRepository = classificationRepository;
        }

        public Classification GetDataById(int id)
        {
            return this.classificationRepository.Get(id);
        }

        public List<Classification> GetDataListByStatus(bool active)
        {
            return this.classificationRepository.Get(x => x.Active.Equals(active)).ToList();
        }
        public List<Classification> GetDataListWithFilter(bool isActive, int filter)
        {
            return this.classificationRepository.Get(x => x.Active.Equals(isActive) && x.SubProcess.Id.Equals(filter)).ToList();
        }

        public List<Classification> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.classificationRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }

        public List<Classification> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.classificationRepository.GetDataWithPagingAndSearch(searchString, pageNumber, pageSize, out total);
        }

        public void Save(Classification entity)
        {
            this.classificationRepository.Save(entity);
        }

        public int Create(Classification entity)
        {
            return this.classificationRepository.Create(entity);
        }

        public void SaveChanges(Classification entity)
        {
            this.classificationRepository.SaveChanges(entity);
        }

        public void Delete(int id)
        {
            this.classificationRepository.Delete(id);
        }

        public bool CheckDataIfExists(Classification entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Name", entity.Name);
            parameter.Add("SubProcess", entity.SubProcess);
            parameter.Add("Description", entity.Description);
            parameter.Add("Active", entity.Active);
            List<Classification> classification = this.classificationRepository.CheckIfDataExists(parameter);
            if (classification.Count() > 0 )
            {
                return true;
            }
            return false;
        }

        public bool CheckDataIfExists(string param)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", param);
            List<Classification> classification = this.classificationRepository.CheckIfDataExists(parameter);
            if (classification.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckDataAndCodeIfExist(Classification entity)
        {
            if (CheckDataIfExists(entity) || CheckDataIfExists(entity.Code))
            {
                return true;
            }
            return false;
        }
     
    }
}
