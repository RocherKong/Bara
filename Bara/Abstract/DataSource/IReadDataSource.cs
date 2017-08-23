using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.DataSource
{
    public interface IReadDataSource : IDataSource
    {
        int Weight { get; set; }
    }
}
