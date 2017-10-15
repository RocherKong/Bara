using Bara.Core.Logger;
using Bara.Core.Mapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bara.Test
{

    public class MapperContainerTest
    {
        //static ILoggerFactory loggerFactory = new LoggerFactory();

        [Fact]
        public void LoadBaraMapper()
        {
            var baraMap = MapperContainer.Instance.GetBaraMapper("BaraMapConfig.xml");

        }
    }
}
