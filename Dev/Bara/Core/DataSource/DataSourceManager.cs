using Bara.Abstract.DataSource;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Core;
using Microsoft.Extensions.Logging;
using Bara.Common;
using System.Linq;

namespace Bara.Core.DataSource
{
    public class DataSourceManager : IDataSourceManager
    {
        private ILogger _logger;
        public IBaraMapper BaraMapper { get; }

        /// <summary>
        /// 读数据库的权重筛选器
        /// </summary>
        private WeightFilter<IDataSource> WeightFilter = new WeightFilter<IDataSource>();


        public DataSourceManager(ILoggerFactory loggerFactory, IBaraMapper baraMapper)
        {
            this._logger = loggerFactory.CreateLogger<DataSourceManager>();
            this.BaraMapper = baraMapper;
        }

        public DataSourceManager(IBaraMapper baraMapper)
        {
            this.BaraMapper = baraMapper;
        }

        public IDataSource GetDataSource(DataSourceType type)
        {
            var ReadDataSources = BaraMapper.BaraMapConfig.DataBase.ReadDataSources;
            IDataSource ChosedDataBase;
            if (ReadDataSources.Count > 0)
            {
                var seekList = ReadDataSources.Select(DataSource => new WeightFilter<IDataSource>.WeightSource
                {
                    Source = DataSource,
                    Weight = DataSource.Weight
                });
                ChosedDataBase = WeightFilter.Elect(seekList).Source;

            }
            else
            {
                ChosedDataBase = BaraMapper.BaraMapConfig.DataBase.WriteDataBase;
            }
            _logger.LogDebug($"DataSourceManager GetDataSource Choice: {ChosedDataBase.Name} .");
            return ChosedDataBase;
        }
    }
}
