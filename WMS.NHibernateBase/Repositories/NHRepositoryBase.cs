using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using WMS.Entities;
using NHibernate.Criterion;
using System.Linq.Expressions;
using WMS.Core.Repositories;

namespace WMS.NHibernateBase.Repositories
{
    public abstract class NHRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        #region Session
        protected ISession Session { get { return NHUnitOfWork.Current.Session; } }
        #endregion
        #region Common CRUD Method      
        public void Save(TEntity entity)
        {
            Session.Save(entity);
        }
        public TPrimaryKey Create(TEntity entity)
        {
            TPrimaryKey Id=(TPrimaryKey)Session.Save(entity);
            return Id;
        }
        public void SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
        }
        public void SaveChanges(TEntity entity)
        {
            Session.Update(entity);
        }
        public void Delete(TPrimaryKey id)
        {
            Session.Delete(Session.Load<TEntity>(id));
        }
        public void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }      
        public IEnumerable<TEntity> SaveOrUpdateAll(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Filter
        public TEntity Get(TPrimaryKey key)
        {
            return Session.Get<TEntity>(key);
        }
        public TEntity GetByExpression(Expression<Func<TEntity, bool>> predicate)
        {
            return Session.Query<TEntity>().Where(predicate).FirstOrDefault();
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Session.Query<TEntity>().Where(predicate);
        }
        public IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }    
        #endregion
        #region Search
        public List<TEntity> GetDataWithPaging(int pageIndex, int pageSize, out long total)
        {
            total = Session.CreateCriteria<TEntity>()
                                   .SetProjection(Projections.RowCountInt64())
                                   .FutureValue<Int64>().Value;
            var results = Session.CreateCriteria<TEntity>()
                .SetFirstResult((pageIndex - 1) * pageSize)
                .SetMaxResults(pageSize)
                .Future<TEntity>()
                .ToList<TEntity>();
            return results.ToList();

        }
        public List<TEntity> GetDataWithPagingAndSearch(List<ICriterion> criterion, string searchString, int pageIndex, int pageSize, out long total)
        {
            var session = Session.CreateCriteria<TEntity>();
            foreach (var criteria in criterion)
            { session.Add(criteria);}
            total = GetDataCount(criterion);
            var results = session
                .SetFirstResult((pageIndex - 1) * pageSize)
                .SetMaxResults(pageSize)
                .Future<TEntity>()
                .ToList<TEntity>();
            
            return results.ToList();
        }
        public long GetDataCount(List<ICriterion> criterion)
        {
            var session = Session.CreateCriteria<TEntity>();
            foreach (var criteria in criterion)
            { session.Add(criteria); }
            long total = session.SetProjection(Projections.RowCountInt64()).FutureValue<Int64>().Value;
            return total;
        }
        public List<TEntity> CheckIfDataExists(Dictionary<string, object> parameter)
        {
            DetachedCriteria criteria = DetachedCriteria.For<TEntity>();
            foreach (var item in parameter)
            {
                criteria.Add(
                NHibernate.Criterion.Restrictions.Eq(item.Key, item.Value)
                );
            }
            return this.FindAll(criteria).ToList();
        }

          public List<TEntity> GetFilteredData(Dictionary<string, object> parameter)
        {
            DetachedCriteria criteria = DetachedCriteria.For<TEntity>();
            foreach (var item in parameter)
            {
                criteria.Add(
                NHibernate.Criterion.Restrictions.Eq(item.Key, item.Value)
                );
            }
            return this.FindAll(criteria).ToList();
        }
       
        #endregion
        #region Criteria
        public IEnumerable<TEntity> FindAll(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).List<TEntity>();
        }
        public IEnumerable<TEntity> FindAll(DetachedCriteria criteria, params Order[] orders)
        {
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    criteria.AddOrder(order);
                }
            }

            return FindAll(criteria);
        }
        public IEnumerable<TEntity> FindAll(DetachedCriteria criteria, int firstResult, int numberOfResults, params Order[] orders)
        {
            criteria.SetFirstResult(firstResult).SetMaxResults(numberOfResults);
            return FindAll(criteria, orders);
        }
        public TEntity FindOne(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).UniqueResult<TEntity>();
        }
        public TEntity FindFirst(DetachedCriteria criteria)
        {
            var results = criteria.SetFirstResult(0).SetMaxResults(1)
                .GetExecutableCriteria(Session).List<TEntity>();

            if (results.Count > 0)
            {
                return results[0];
            }

            return default(TEntity);
        }
        public TEntity FindFirst(DetachedCriteria criteria, Order order)
        {
            return FindFirst(criteria.AddOrder(order));
        }
        public long Count(DetachedCriteria criteria)
        {
            return Convert.ToInt64(criteria.GetExecutableCriteria(Session)
                .SetProjection(Projections.RowCountInt64()).UniqueResult());
        }
        public bool Exists(DetachedCriteria criteria)
        {
            return Count(criteria) > 0;
        }
        #endregion
       
    }
}
