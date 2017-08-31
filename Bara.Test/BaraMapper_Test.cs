using Bara.Core.Mapper;
using Bara.Test.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bara.Test
{
    public class BaraMapper_Test
    {
        public BaraMapper mapper { get; private set; }
        public BaraMapper_Test()
        {
            mapper = new BaraMapper(baraMapConfigFilePath: "BaraMapConfig.xml");
        }
        [Fact]
        public void BaraMapperLoader_Test()
        {
            int i = mapper.Execute(new Core.Context.RequestContext
            {
                Scope = "T_Test",
                SqlId = "Insert",
                Request = new { Id = 1, Name = "Rocher" }
            });

            Assert.IsType<BaraMapper>(mapper);
        }

        [Fact]
        public void QuerySingle_Test()
        {
            var result = mapper.QuerySingle<T_Test>(new Core.Context.RequestContext
            {
                SqlId = "GetEntity",
                Scope = "T_Test",
                Request = new { Id = 1 }
            });


        }
    }
}
