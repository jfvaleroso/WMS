using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Services.IServices;
using WMS.Entities;
using WMS.Core.Repositories;

namespace WMS.Core.Services.Implementation
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;
        public DocumentService(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public Document GetDataById(int id)
        {
            return this.documentRepository.Get(id);
        }

        public List<Document> GetDataListByStatus(bool active)
        {
            return this.documentRepository.Get(x => x.Active.Equals(active)).ToList();
        }

        public List<Document> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.documentRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }

        public List<Document> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.documentRepository.GetDataWithPagingAndSearch(searchString, pageNumber, pageSize, out total);
        }

        public void Save(Document entity)
        {
            this.documentRepository.Save(entity);
        }

        public int Create(Document entity)
        {
            return this.documentRepository.Create(entity);
        }

        public void SaveChanges(Document entity)
        {
            this.documentRepository.SaveChanges(entity);
        }

        public void Delete(int id)
        {
            this.documentRepository.Delete(id);
        }

        public bool CheckDataIfExists(Document entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Name", entity.Name);
            parameter.Add("Code", entity.Code);
            parameter.Add("Active", entity.Active);
            parameter.Add("Description", entity.Description);
            List<Document> document = this.documentRepository.CheckIfDataExists(parameter);
            if (document.Count() > 0 )
            {
                return true;
            }
            return false;
        }

        public bool CheckDataIfExists(string param)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Code", param);
            List<Document> document = this.documentRepository.CheckIfDataExists(parameter);
            if (document.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckDataAndCodeIfExist(Document entity)
        {
            if (CheckDataIfExists(entity) || CheckDataIfExists(entity.Code))
            {
                return true;
            }
            return false;
        }


        public List<Document> GetDataListWithFilter(bool isActive, int filter)
        {
            return this.documentRepository.Get(x => x.Active.Equals(isActive)).ToList();
        }
    }
}
