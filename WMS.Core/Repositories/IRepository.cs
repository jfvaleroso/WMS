using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using WMS.Entities;



namespace WMS.Core.Repositories
{
    public interface IRepository
    {}

    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : Entity<TPrimaryKey>
    {
        #region Paging
        TEntity Get(TPrimaryKey key);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetDataWithPaging(int pageNumber, int pageSize, out long total);
        List<TEntity> CheckIfDataExists(Dictionary<string, object> parameter);
        List<TEntity> GetFilteredData(Dictionary<string, object> parameter);

        #endregion
        #region Common Method
        void Save(TEntity entity);
        TPrimaryKey Create(TEntity entity);
        void SaveOrUpdate(TEntity entity);
        void SaveChanges(TEntity entity);
        void Delete(TPrimaryKey id);
        void Delete(TEntity entity);
        IEnumerable<TEntity> SaveOrUpdateAll(params TEntity[] entities);
        #endregion


    }
}
