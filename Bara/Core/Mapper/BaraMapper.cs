using Bara.Abstract.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Model;
using Bara.Core.Logger;
using Microsoft.Extensions.Logging;
using Bara.Abstract.Config;
using Bara.Common;

namespace Bara.Core.Mapper
{
    public class BaraMapper : IBaraMapper
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        public IConfigLoader ConfigLoader { get; }

        public BaraMapper(String baraMapConfigFilePath = "BaraMapConfig.xml") : this(NullLoggerFactory.Instance,baraMapConfigFilePath)
        {

        }

        public BaraMapper(ILoggerFactory loggerFactory,String baraMapConfigFilePath="BaraMapConfig.xml")
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<BaraMapper>();
            ConfigLoader = new LocalConfigLoader(loggerFactory);
            ConfigLoader.Load(baraMapConfigFilePath, this);
            var DbProviderFactory = BaraMapConfig.DataBase.DbProvider.DbProviderFactory;




        }

        public BaraMapConfig BaraMapConfig { get; private set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void LoadConfig(BaraMapConfig config)
        {
            BaraMapConfig = config;
        }


    }
}
