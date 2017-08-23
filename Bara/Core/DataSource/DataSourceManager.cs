using Bara.Abstract.DataSource;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Core;
using Microsoft.Extensions.Logging;

namespace Bara.Core.DataSource
{
    public class DataSourceManager : IDataSourceManager
    {
        public IBaraMapper BaraMapper { get; }

        public DataSourceManager(ILoggerFactory loggerFactory,IBaraMapper baraMapper)
        {
            this.BaraMapper = baraMapper;
        }

        public IDataSource GetDataSource(DataSourceType type)
        {
            throw new NotImplementedException();
        }
    }
}
