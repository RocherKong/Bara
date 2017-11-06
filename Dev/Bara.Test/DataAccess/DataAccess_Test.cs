
using Bara.Test.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bara.Test.DataAccess
{
    public class DataAccess_Test : TestBase
    {
        private T_TestDataAccess _dao_test;
        public DataAccess_Test()
        {
            _dao_test = new T_TestDataAccess(_baraMapper);
        }
        [Fact]
        public void Insert_Test()
        {
            var id = _dao_test.Insert<long>(new Entity.T_Test
            {
                Name = "Rocher Kong"
            });
        }

        [Fact]
        public void GetEntity()
        {
            var resp = _dao_test.GetEntity(1);
        }

        [Fact]
        public void GetEntityCache_Test()
        {
            for (int i = 0; i < 3; i++)
            {
                _dao_test.GetEntity(1);
            }
        }

        [Fact]
        public void GetEntityObject()
        {
            var resp = _dao_test.GetSingleEntity(new { Id = 2 });

        }

        [Fact]
        public void QueryList_Test()
        {
            var resp = _dao_test.QueryList<T_Test>(new { });
        }
    }
}
