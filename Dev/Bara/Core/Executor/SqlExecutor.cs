using Bara.Abstract.Executor;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.DataSource;
using Bara.Abstract.Session;
using Bara.Core.Context;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Bara.Abstract.Core;
using Bara.Abstract.Builder;

namespace Bara.Core.Executor
{
    public class SqlExecutor : ISqlExecutor
    {
        ILogger _logger;
        ISqlBuilder _sqlBuilder;
        IBaraMapper _baraMapper;
        public SqlExecutor(ILoggerFactory loggerFactory, ISqlBuilder sqlBuilder, IBaraMapper baraMapper)
        {
            _logger = loggerFactory.CreateLogger<SqlExecutor>();
            _sqlBuilder = sqlBuilder;
            _baraMapper = baraMapper;
        }

        public SqlExecutor(ISqlBuilder sqlBuilder, IBaraMapper baraMapper)
        {
            _sqlBuilder = sqlBuilder;
            _baraMapper = baraMapper;
        }
        public T Execute<T>(RequestContext context, DataSourceType dataSourceType, Func<string, IDbConnectionSession, T> executeSql)
        {
            IDbConnectionSession session = _baraMapper.SessionStore.LocalSession;
            if (session == null)
            {
                session = _baraMapper.CreateDbSession(dataSourceType);
            }
            try
            {
                var strSql = _sqlBuilder.BuildSql(context);
                return executeSql(strSql, session);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (session.LifeCycle == DbSessionLifeCycle.Transient)
                {
                    session.CloseConnection();
                }
            }

        }

        public async Task<T> ExecuteAsync<T>(RequestContext context, DataSourceType dataSourceType, Func<string, IDbConnectionSession, Task<T>> executeSql)
        {
            IDbConnectionSession session = _baraMapper.SessionStore.LocalSession;
            if (session == null)
            {
                session = _baraMapper.CreateDbSession(dataSourceType);
            }
            try
            {
                var strSql = _sqlBuilder.BuildSql(context);
                return await executeSql(strSql, session);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (session.LifeCycle == DbSessionLifeCycle.Transient)
                {
                    session.CloseConnection();
                }
            }
        }
    }
}
