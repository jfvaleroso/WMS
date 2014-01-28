using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using System.Data;
using WMS.Core.UnitofWork;

namespace WMS.NHibernateBase.Repositories
{
    public class NHUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets current instance of the NhUnitOfWork.
        /// It gets the right instance that is related to current thread.
        /// </summary>
        /// 
        [ThreadStatic]
        private static NHUnitOfWork _current;
        public static NHUnitOfWork Current
        {
            get { return _current; }
            set { _current = value; }
        }
       

        /// <summary>
        /// Gets Nhibernate session object to perform queries.
        /// </summary>
        public ISession Session { get; private set; }



        /// <summary>
        /// Reference to the session factory.
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Reference to the currently running transcation.
        /// </summary>
        private ITransaction _transaction;

        /// <summary>
        /// Creates a new instance of NhUnitOfWork.
        /// </summary>
        /// <param name="sessionFactory"></param>
        public NHUnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            Session = _sessionFactory.OpenSession();
        }
       
        /// <summary>
        /// Opens database connection and begins transaction.
        /// </summary>
        public void BeginTransaction()
        {
            //Session = _sessionFactory.OpenSession();
            //_transaction = Session.BeginTransaction();
            if (_transaction == null)
            {
                _transaction = Session.BeginTransaction();
            }
            else
            {
                if (!_transaction.IsActive)
                    _transaction = Session.BeginTransaction();
            }
        }

        /// <summary>
        /// Commits transaction and closes database connection.
        /// </summary>
        public void Commit()
        {
            try
            {
                if (_transaction.IsActive)
                    _transaction.Commit();
  
            }
            finally
            {
                
            }
        }

      

        /// <summary>
        /// Rollbacks transaction and closes database connection.
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (_transaction.IsActive)
                _transaction.Rollback();
            }
            finally
            {
              
            }
        }

        public void Dispose()
        {
            Session.Close();
        }

      
    }


    //public class NHUnitOfWork : IUnitOfWork
    //{
    //    private readonly ISessionFactory _sessionFactory;
    //    public static ISession _currentSession;
    //    private ITransaction _currentTransaction;


    //      public static NHUnitOfWork Current
    //    {
    //        get { return _current; }
    //        set { _current = value; }
    //    }
    //      [ThreadStatic]
    //      private static NHUnitOfWork _current;



    //    public NHUnitOfWork(ISessionFactory sessionFactory)
    //    {
    //        _sessionFactory = sessionFactory;
    //        _currentTransaction = CurrentSession.BeginTransaction();
    //    }



    //    #region IUnitOfWork Members

    //    public ISession CurrentSession
    //    {
    //        get { return _currentSession ?? (_currentSession = _sessionFactory.OpenSession()); }
    //    }

    //    public void BeginTransaction()
    //    {
    //        _currentTransaction= _currentSession.BeginTransaction();
    //    }

    //    public void Commit()
    //    {
    //        if (_currentTransaction.IsActive)
    //            _currentTransaction.Commit();
    //    }
    //    public void Rollback()
    //    {
    //        if (_currentTransaction.IsActive)
    //            _currentTransaction.Rollback();
    //    }

    //    #endregion

    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //        if (_currentSession == null) return;

    //        _currentSession.Dispose();
    //        _currentSession = null;
    //    }

    //    #endregion
    //}
}
