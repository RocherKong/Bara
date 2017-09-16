using Bara.Abstract.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.DataSource
{
    public interface IDataSourceManager
    {
        IBaraMapper BaraMapper { get; }

        IDataSource GetDataSource(DataSourceType type);
    }
}
