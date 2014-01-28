using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Repositories;
using WMS.Core.Services.IServices;
using WMS.Entities;

namespace WMS.Core.Services.Implementation
{
     public class NotificationMappingService : INotificationMappingService
    {
        private readonly INotificationMappingRepository notificationMappingRepository;
        public NotificationMappingService(INotificationMappingRepository notificationMappingRepository)
        {
            this.notificationMappingRepository = notificationMappingRepository;
        }

        public NotificationMapping GetDataById(long id)
        {
            return this.notificationMappingRepository.Get(id);
        }

        public List<NotificationMapping> GetByExpression(long id)
        {
            return this.notificationMappingRepository.Get(x => x.Workflow.Id.Equals(id)).ToList();
        }

        public List<NotificationMapping> GetDataListByStatus(bool active)
        {
            return this.notificationMappingRepository.Get(x => x.Active.Equals(active)).ToList();
        }

        public List<NotificationMapping> GetDataListWithPaging(int pageNumber, int pageSize, out long total)
        {
            return this.notificationMappingRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }

        public List<NotificationMapping> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total)
        {
            return this.notificationMappingRepository.GetDataWithPaging(pageNumber, pageSize, out total);
        }


        public void Save(NotificationMapping entity)
        {
            this.notificationMappingRepository.Save(entity);
        }

        public long Create(NotificationMapping entity)
        {
            return this.notificationMappingRepository.Create(entity);
        }

        public void SaveChanges(NotificationMapping entity)
        {
            this.notificationMappingRepository.SaveChanges(entity);
        }

        public void Delete(long id)
        {
            this.notificationMappingRepository.Delete(id);
        }

        public bool CheckDataIfExists(NotificationMapping entity)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", entity.Workflow);
            parameter.Add("Status", entity.Status);
            List<NotificationMapping> document = this.notificationMappingRepository.CheckIfDataExists(parameter);
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
        public bool CheckDataAndCodeIfExist(NotificationMapping entity)
        {

            return false;
        }

        public List<NotificationMapping> GetDataListWithFilter(bool isActive, int filter)
        {
            return this.notificationMappingRepository.Get(x => x.Active.Equals(isActive)).ToList();
        }

        public List<NotificationMapping> GetFilteredDataByWorkflow(Workflow workflow)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", workflow);
            List<NotificationMapping> notificationList = this.notificationMappingRepository.GetFilteredData(parameter);
            return notificationList;
        }


        public NotificationMapping GetNotificationMappingByWorkflowAndStatus(Workflow workflow, Status status)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Workflow", workflow);
            parameter.Add("Status", status);
            NotificationMapping notification = this.notificationMappingRepository.GetFilteredData(parameter).FirstOrDefault();
            return notification;
        }

        public void DeleteNotificationMapping(Workflow workflow)
        {
            List<NotificationMapping> notificationMappingList = new List<NotificationMapping>();
            notificationMappingList = GetFilteredDataByWorkflow(workflow);
            if (notificationMappingList.Any())
            {
                foreach (var notificationMapping in notificationMappingList)
                {
                    this.notificationMappingRepository.Delete(notificationMapping);
                }
            }

        }
    }
}

