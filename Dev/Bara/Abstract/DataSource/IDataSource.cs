using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.DataSource
{
    public interface IDataSource
    {
        String Name { get; set; }

        String ConnectionString { get; set; }
    }
}
