using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Repositories;
using WMS.Core.Services.IServices;
using WMS.Entities;

namespace WMS.Core.Services.Implementation
{
    public class DocumentMappingService : IDocumentMappingService
    {
        private readonly IDocumentMappingRepository documentMappingRepository;
        public DocumentMappingService(IDocumentMappingRepository documentMappingRepository)
        {
            this.documentMappingRepository = documentMappingRepository;
        }

        public DocumentMapping GetDataById(long id)
        {
            return this.documentMappingRepository.Get(id);
        }

        public List<DocumentMapping> GetByExpression(long id)
        {
            return this.documentMappingRepository.Get(x => x.Workflow.Id.Equals(id)).ToList();
        }

        public List<DocumentMapping> GetDataListByStatus(bool active)
        {
            return this.documentMappingRepository.Get(x => x.Active.Equals(active)).ToList();
        }

        public List<DocumentMapping> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.documentMappingRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }

        public List<DocumentMapping> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.documentMappingRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }


        public void Save(DocumentMapping entity)
        {
            this.documentMappingRepository.Save(entity);
        }

        public long Create(DocumentMapping entity)
        {
            return this.documentMappingRepository.Create(entity);
        }

        public void SaveChanges(DocumentMapping entity)
        {
            this.documentMappingRepository.SaveChanges(entity);
        }

        public void Delete(long id)
        {
            this.documentMappingRepository.Delete(id);
        }

        public bool CheckDataIfExists(DocumentMapping entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", entity.Workflow);
            parameter.Add("Document", entity.Document);
            List<DocumentMapping> document = this.documentMappingRepository.CheckIfDataExists(parameter);
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

        public bool CheckDataAndCodeIfExist(DocumentMapping entity)
        {
            
            return false;
        }


        public bool CheckDataAndCodeIfExist(Status entity)
        {
   
            return false;
        }


        public List<DocumentMapping> GetDataListWithFilter(bool isActive, int filter)
        {
            return this.documentMappingRepository.Get(x => x.Active.Equals(isActive)).ToList();
        }

        public List<DocumentMapping> GetFilteredDataByWorkflow(Workflow workflow)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", workflow);
            List<DocumentMapping> documentList = this.documentMappingRepository.GetFilteredData(parameter);
            return documentList;
        }

        public void DeleteDocumentMapping(Workflow workflow)
        {
            List<DocumentMapping> documentMappingList= new List<DocumentMapping>();
            documentMappingList= GetFilteredDataByWorkflow(workflow);
            if (documentMappingList.Any())
            {
                foreach (var documentMapping in documentMappingList)
                {
                    this.documentMappingRepository.Delete(documentMapping);
                }
            }  

        }
    }
}
