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
            Assert.IsType<BaraMapper>(mapper);
        }
    }
}
