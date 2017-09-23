using Bara.Abstract.DataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Bara.Abstract.Session
{
    public interface IDbConnectionSession : IDisposable
    {
        Guid Id { get; }

        DbProviderFactory DbProviderFactory { get; }

        IDataSource DataSource { get; }

        IDbConnection Connection { get; }

        IDbConnection CreateConnection();

        IDbTransaction DbTransaction { get; }

        void OpenConnection();

        void CloseConnection();

        DbSessionLifeCycle LifeCycle { get;set; }

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void RollbackTransaction();
    }
}
