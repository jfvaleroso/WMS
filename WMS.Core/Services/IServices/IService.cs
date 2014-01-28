using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.Core.Services.IServices
{
    public interface IService<TEntity, TPrimaryKey>
    {
        TEntity GetDataById(TPrimaryKey id);
        List<TEntity> GetDataListByStatus(bool isActive);
        List<TEntity> GetDataListWithPaging(int pageNumber, int pageSize, out long total);
        List<TEntity> GetDataListWithPagingAndSearch(string searchString, int pageNumber, int pageSize, out long total);
        bool CheckDataIfExists(TEntity entity);
        bool CheckDataAndCodeIfExist(TEntity entity);
        bool CheckDataIfExists(string param);
        void Save(TEntity entity);
        TPrimaryKey Create(TEntity entity);
        void SaveChanges(TEntity entity);
        void Delete(TPrimaryKey id);
    }
}
