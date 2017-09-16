using Bara.Abstract.DataSource;
using Bara.Abstract.Session;
using Bara.Core.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bara.Abstract.Executor
{
    public interface ISqlExecutor
    {
        T Execute<T>(RequestContext context, DataSourceType dataSourceType, Func<String, IDbConnectionSession, T> executeSql);

        Task<T> ExecuteAsync<T>(RequestContext context, DataSourceType dataSourceType, Func<String, IDbConnectionSession, Task<T>> executeSql);
    }
}
