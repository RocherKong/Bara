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
