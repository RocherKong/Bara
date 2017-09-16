using Bara.Abstract.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Model;
using Bara.Core.Logger;
using Microsoft.Extensions.Logging;
using Bara.Abstract.Config;
using Bara.Common;
using System.Data.Common;
using Bara.Abstract.Session;
using Bara.Core.Session;
using Bara.Abstract.Builder;
using Bara.Core.Builder;
using Bara.Abstract.DataSource;
using Bara.Core.DataSource;
using Bara.Abstract.Executor;
using Bara.Core.Executor;
using Bara.Core.Context;
using Dapper;
using System.Threading.Tasks;
using Bara.Abstract.Cache;
using Bara.Core.Cache;

namespace Bara.Core.Mapper
{
    public class BaraMapper : IBaraMapper
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        public IConfigLoader ConfigLoader { get; }

        public DbProviderFactory DbProviderFactory { get; }

        public IDbConnectionSessionStore SessionStore { get; }

        public ISqlBuilder SqlBuilder { get; }

        public IDataSourceManager DataSourceManager { get; }

        public ISqlExecutor SqlExecutor { get; }

        public BaraMapConfig BaraMapConfig { get; private set; }

        public ICacheManager CacheManager { get; }
        public BaraMapper(String baraMapConfigFilePath = "BaraMapConfig.xml") : this(NullLoggerFactory.Instance, baraMapConfigFilePath)
        {

        }

        public BaraMapper(ILoggerFactory loggerFactory, String baraMapConfigFilePath = "BaraMapConfig.xml")
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<BaraMapper>();
            ConfigLoader = new LocalConfigLoader(loggerFactory);
            ConfigLoader.Load(baraMapConfigFilePath, this);
            DbProviderFactory = BaraMapConfig.DataBase.DbProvider.DbProviderFactory;
            SessionStore = new DbConnectionSessionStore(loggerFactory, this.GetHashCode().ToString());
            SqlBuilder = new SqlBuilder(loggerFactory, this);
            DataSourceManager = new DataSourceManager(loggerFactory, this);
            CacheManager = new CacheManager(loggerFactory, this);
            SqlExecutor = new SqlExecutor(loggerFactory, SqlBuilder, this);
        }

        public void LoadConfig(BaraMapConfig config)
        {
            BaraMapConfig = config;
        }

        public IDbConnectionSession CreateDbSession(DataSourceType dataSourceType)
        {
            IDataSource dataSource = DataSourceManager.GetDataSource(dataSourceType);
            IDbConnectionSession session = new DbConnectionSession(_loggerFactory, DbProviderFactory, dataSource);
            session.CreateConnection();
            return session;
        }

        #region Sync
        public int Execute(RequestContext context)
        {

            int result = SqlExecutor.Execute<int>(context, DataSourceType.Write, (strsql, session) =>
            {
                return session.Connection.Execute(strsql, context.Request, session.DbTransaction);
            });

            return result;
        }

        //
        // 摘要:
        //     Execute parameterized SQL that selects a single value
        //
        // 返回结果:
        //     The first cell selected
        public T ExecuteScalar<T>(RequestContext context)
        {
            T result = SqlExecutor.Execute<T>(context, DataSourceType.Read, (sqlStr, session) =>
            {
                return session.Connection.ExecuteScalar<T>(sqlStr, context.Request, session.DbTransaction);
            });
            return result;
        }

        public T QuerySingle<T>(RequestContext context, DataSourceType dataSourceType = DataSourceType.Read)
        {
            #region Read Cache
            var cache = CacheManager[context, typeof(T)];
            if (cache != null)
            {
                return (T)cache;
            }
            #endregion
            T result = SqlExecutor.Execute<T>(context, dataSourceType, (sqlStr, session) =>
            {
                return session.Connection.QuerySingleOrDefault<T>(sqlStr, context.Request, session.DbTransaction);
            });
            CacheManager[context, typeof(T)] = result;
            return result;
        }

        public IEnumerable<T> Query<T>(RequestContext context, DataSourceType sourceType = DataSourceType.Read)
        {
            #region Read Cache
            var cache = CacheManager[context, typeof(IEnumerable<T>)];
            if (cache != null)
            {
                return (IEnumerable<T>)cache;
            }
            #endregion

            IDbConnectionSession session = SessionStore.LocalSession;
            if (session == null)
            {
                session = CreateDbSession(sourceType);
            }
            string sqlStr = SqlBuilder.BuildSql(context);
            try
            {
                var result = session.Connection.Query<T>(sqlStr, context.Request, session.DbTransaction);
                CacheManager[context, typeof(IEnumerable<T>)] = result;
                return result;
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
        #endregion

        #region Async
        public async Task<int> ExecuteAsync(RequestContext context)
        {
            int result = await SqlExecutor.ExecuteAsync<int>(context, DataSourceType.Write, (sqlstr, session) =>
            {
                return session.Connection.ExecuteAsync(sqlstr, context.Request, session.DbTransaction);
            });
            return result;
        }

        public async Task<T> ExecuteScalarAsync<T>(RequestContext context)
        {
            #region Read Cache
            var cache = CacheManager[context, typeof(T)];
            if (cache != null)
            {
                return (T)cache;
            }
            #endregion
            T result = await SqlExecutor.ExecuteAsync<T>(context, DataSourceType.Read, (sqlStr, session) =>
             {
                 return session.Connection.ExecuteScalarAsync<T>(sqlStr, context.Request, session.DbTransaction);
             });
            CacheManager[context, typeof(T)] = result;
            return result;
        }

        public async Task<T> QuerySingleAsync<T>(RequestContext context, DataSourceType dataSourceType = DataSourceType.Read)
        {
            T result = await SqlExecutor.ExecuteAsync<T>(context, dataSourceType, (sqlStr, session) =>
             {
                 return session.Connection.QuerySingleOrDefaultAsync<T>(sqlStr, context.Request, session.DbTransaction);
             });
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(RequestContext context, DataSourceType sourceType = DataSourceType.Read)
        {
            #region Read Cache
            var cache = CacheManager[context, typeof(IEnumerable<T>)];
            if (cache != null)
            {
                return (IEnumerable<T>)cache;
            }
            #endregion
            IDbConnectionSession session = SessionStore.LocalSession;
            if (session == null)
            {
                session = CreateDbSession(sourceType);
            }
            string sqlStr = SqlBuilder.BuildSql(context);
            try
            {
                var result = await session.Connection.QueryAsync<T>(sqlStr, context.Request, session.DbTransaction);
                CacheManager[context, typeof(IEnumerable<T>)] = result;
                return result;
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
        #endregion

        public void Dispose()
        {
            ConfigLoader?.Dispose();
            if (SessionStore != null)
            {
                SessionStore.LocalSession?.Dispose();
                SessionStore.Dispose();
            }
        }


    }
}
