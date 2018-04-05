using Bara.Abstract.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Bara.DataAccess.Abstractions
{
    public interface IWrite<TEntity> : IInsert<TEntity>, IDelete, IUpdate<TEntity> where TEntity : class
    {
    }

    public interface IInsert<TEntity> where TEntity : class
    {
        TPrimary Insert<TPrimary>(TEntity entity);
        void Insert(TEntity entity);
    }

    public interface IDelete
    {
        int Delete<TPrimary>(TPrimary Id);
    }

    public interface IUpdate<TEntity> where TEntity : class
    {
        int Update(TEntity entity);

        int DynamicUpdate(object entity);
    }

    public interface ITransaction
    {
        IDbConnectionSession BeginTransaction();
        IDbConnectionSession BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void RollbackTransaction();
    }
}
