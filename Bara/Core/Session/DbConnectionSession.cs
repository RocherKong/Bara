using Bara.Abstract.Session;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.DataSource;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using Bara.Exceptions;

namespace Bara.Core.Session
{
    public class DbConnectionSession : IDbConnectionSession
    {
        private readonly ILogger _logger;
        public Guid Id { get; }

        public DbProviderFactory DbProviderFactory { get; }

        public IDataSource DataSource { get; }

        public IDbConnection Connection { get; private set; }

        public DbSessionLifeCycle LifeCycle { get; private set; }

        public IDbTransaction DbTransaction { get; private set; }

        public DbConnectionSession(ILoggerFactory loggerFactory, DbProviderFactory dbProviderFactory, IDataSource dataSource)
        {
            this._logger = loggerFactory.CreateLogger<DbConnectionSession>();
            this.DbProviderFactory = dbProviderFactory;
            this.DataSource = dataSource;
        }

        public void BeginTransaction()
        {
            OpenConnection();
            Connection.BeginTransaction();
            LifeCycle = DbSessionLifeCycle.Scoped;
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            OpenConnection();
            Connection.BeginTransaction(isolationLevel);
            LifeCycle = DbSessionLifeCycle.Scoped;
        }

        public void CloseConnection()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
                Connection.Dispose();
            }
            Connection = null;
        }

        public void CommitTransaction()
        {
            if (DbTransaction.Connection.State == ConnectionState.Open)
            {
                DbTransaction.Commit();
                DbTransaction.Dispose();
                DbTransaction = null;
                LifeCycle = DbSessionLifeCycle.Transient;
                CloseConnection();
            }
            else
            {
                _logger.LogError($"Transaction Can't Commit Because DbConnection of Trans:{DbTransaction.GetHashCode()} is Not Open");

            }
        }

        public IDbConnection CreateConnection()
        {
            Connection = DbProviderFactory.CreateConnection();
            Connection.ConnectionString = DataSource.ConnectionString;
            return Connection;
        }

        public void Dispose()
        {
            if (DbTransaction != null)
            {
                if (Connection.State != ConnectionState.Closed)
                {
                    RollbackTransaction();
                }
            }
            else
            {
                CloseConnection();
            }
        }

        public void OpenConnection()
        {
            if (Connection == null)
            {
                CreateConnection();
                OpenConnectDb();

            }
            else
            {
                if (Connection.State != ConnectionState.Open)
                {
                    OpenConnectDb();
                }
            }
        }

        private void OpenConnectDb()
        {
            try
            {
                _logger.LogDebug($"OpenConnection {Connection.GetHashCode()} to {DataSource.Name} .");
                Connection.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError($"OpenConnection Unable to open connection to { DataSource.Name }.");
                throw new BaraException($"OpenConnection Unable to open connection to { DataSource.Name }.", ex);
            }
        }

        public void RollbackTransaction()
        {
            DbTransaction?.Rollback();
            DbTransaction?.Dispose();
            DbTransaction = null;
            LifeCycle = DbSessionLifeCycle.Transient;
            CloseConnection();
        }
    }
}
