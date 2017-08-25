using Bara.Core.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bara.Test
{
    public class BaraMapper_Test
    {
        [Fact]
        public void BaraMapperLoader_Test()
        {
            BaraMapper mapper = new BaraMapper(baraMapConfigFilePath: "BaraMapConfig.xml");

            int i = mapper.Execute(new Core.Context.RequestContext
            {
                Scope = "T_Test",
                SqlId = "Insert",
                Request = new { Id = 1 }
            });

            Assert.IsType<BaraMapper>(mapper);
        }
    }
}
