using Bara.Core.Mapper;
using Bara.Test.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Bara.Test
{
    public class BaraMapper_Test
    {
        public BaraMapper mapper { get; private set; }
        public BaraMapper_Test()
        {
            //.ConfigureNLog("Nlog.config")
            ILoggerFactory loggerFactory = new LoggerFactory().AddNLog();
            loggerFactory.ConfigureNLog("Nlog.config");

            mapper = new BaraMapper(loggerFactory, baraMapConfigFilePath: "BaraMapConfig.xml");
        }
        [Fact]
        public void BaraMapperLoader_Test()
        {

            int i = mapper.Execute(new Core.Context.RequestContext
            {
                Scope = "T_Test",
                SqlId = "Insert",
                Request = new { Id = 2, Name = "Rocher2" }
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
