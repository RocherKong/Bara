using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Session
{
    public interface IDbConnectionSession : IDisposable
    {
        Guid Id { get; }

        DbProviderFactory DbProviderFactory { get; }

        IDataSource DataSource { get; }

        IDbConnection Connection { get; }

        void CreateConnection();

        void OpenConnection();

        void CloseConnection();

        DbSessionLifeCycle LifeCycle { get; }

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void RollbackTransaction();
    }
}
