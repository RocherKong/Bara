using Bara.Abstract.Builder;
using Bara.Abstract.DataSource;
using Bara.Abstract.Session;
using Bara.Core.Context;
using Bara.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Bara.Abstract.Core
{
    public interface IBaraMapper : IDisposable
    {
        BaraMapConfig BaraMapConfig { get; }
        void LoadConfig(BaraMapConfig config);

        IDbConnectionSessionStore SessionStore { get; }

        ISqlBuilder SqlBuilder { get; }

        IDataSourceManager DataSourceManager { get; }

        IDbConnectionSession CreateDbSession(DataSourceType dataSourceType);

        int Execute(RequestContext context);

        T ExecuteScalar<T>(RequestContext context);

        T QuerySingle<T>(RequestContext context, DataSourceType dataSourceType);

        IEnumerable<T> Query<T>(RequestContext context, DataSourceType dataSourceType);

        #region Transaction
        IDbConnectionSession BeginTransaction();
        IDbConnectionSession BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollbackTransaction();
        #endregion
        #region Scoped Session
        IDbConnectionSession BeginSession(DataSourceType sourceType = DataSourceType.Write);
        void EndSession();
        #endregion

    }
}
