using Bara.Abstract.Builder;
using Bara.Abstract.DataSource;
using Bara.Abstract.Session;
using Bara.Core.Context;
using Bara.Model;
using System;
using System.Collections.Generic;
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

        T QuerySingle<T>(RequestContext context);

        IEnumerable<T> Query<T>(RequestContext context, DataSourceType dataSourceType);



    }
}
