using Bara.Abstract.Config;
using Bara.Abstract.Core;
using Bara.Common;
using Bara.Core.Logger;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Mapper
{
    public class MapperContainer : IDisposable
    {
        private MapperContainer() { }

        private static readonly Object syncObj = new Object();

        public static MapperContainer Instance = new MapperContainer();

        private IDictionary<String, IBaraMapper> _mapperContainer = new Dictionary<String, IBaraMapper>();

        public IBaraMapper GetBaraMapper(String BaraMapConfigPath = "BaraMapConfig.xml")
        {
            return GetBaraMapper(NullLoggerFactory.Instance, BaraMapConfigPath, new LocalConfigLoader());
        }

        public IBaraMapper GetBaraMapper(ILoggerFactory loggerFactory, String baraMapConfigPath, IConfigLoader configLoader)
        {
            if (!_mapperContainer.ContainsKey(baraMapConfigPath))
            {
                lock (syncObj)
                {
                    if (!_mapperContainer.ContainsKey(baraMapConfigPath))
                    {
                        IBaraMapper _mapper = new BaraMapper(loggerFactory, baraMapConfigPath, configLoader);
                        _mapperContainer.Add(baraMapConfigPath, _mapper);
                    }
                }
            }
            return _mapperContainer[baraMapConfigPath];
        }

        public IBaraMapper GetBaraMapper(ILoggerFactory loggerFactory, String baraMapConfigPath)
        {
            return GetBaraMapper(loggerFactory, baraMapConfigPath, new LocalConfigLoader());
        }

        public void Dispose()
        {
            foreach (var mapper in _mapperContainer)
            {
                mapper.Value.Dispose();
            }
            _mapperContainer.Clear();
        }
    }
}
