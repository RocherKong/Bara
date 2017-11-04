using Bara.Core.Mapper;
using Bara.Test.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xunit;
namespace Bara.Test
{
    public class QueryBySqlTest : TestBase
    {
        [Fact]
        public void QueryBySql_Test()
        {
            var result = _baraMapper.QueryBySql("Select Id as '编号',Name as '姓名' from T_Test", null);
            var resultList = result.ToList();
        }

        [Fact]
        public void QueryDataReader_Test()
        {
            var dataReader = _baraMapper.ExecuteReader("Select Id as '编号',Name as '姓名' from T_Test", null);

            //DataTable dataTable = new DataTable();
            //dataTable.Load(dataReader);
            //DataTable dataTable = new DataTable(dataReader);
        }

    }
}
