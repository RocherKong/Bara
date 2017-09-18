using Bara.Abstract.Config;
using Bara.Abstract.Core;
using Bara.Core.Config;
using Bara.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bara.Common
{
    public class LocalConfigLoader : ConfigLoader
    {
        private readonly ILogger _logger;
        public LocalConfigLoader(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LocalConfigLoader>();
        }

        public LocalConfigLoader()
        {
        }

        //总加载器
        //1初始化配置文件（BaraMapConfig），2监控文件BaraMapper   调用器
        public override BaraMapConfig Load(String filePath, IBaraMapper baraMapper)
        {
            _logger.LogDebug($"加载配置文件{filePath}");
            var configStream = LoadConfigStream(filePath);
            var Config = LoadConfig(configStream, baraMapper);
            foreach (var baraMapSource in Config.BaraMapSources)
            {
                LoadBaraMap(baraMapSource.Path, Config);
            }
            baraMapper.LoadConfig(Config);
            if (Config.Settings.IsWatchConfigFile)
            {
                //监控
                WatchConfig(baraMapper);
            }
            return Config;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <param name="baraMapper">Bara核心</param>
        /// <returns></returns>
        public BaraMapConfig LoadConfig(String filePath, IBaraMapper baraMapper)
        {
            //反序列化config
            XmlSerializer serializer = new XmlSerializer(typeof(BaraMapConfig));
            BaraMapConfig config = null;
            using (var configStream = FileLoader.Load(filePath))
            {
                config = serializer.Deserialize(configStream) as BaraMapConfig;
                config.Path = filePath;
                config.BaraMapper = baraMapper;
                // config.BaraMapConfig
            }

            return config;
        }

        /// <summary>
        /// 监控配置文件变化
        /// </summary>
        /// <param name="config"></param>
        public void WatchConfig(IBaraMapper baraMapper)
        {
            var config = baraMapper.BaraMapConfig;

            var ConfigFileInfo = FileLoader.GetFileInfo(config.Path);
            ///监控Config
            FileWatcherLoader.Instance.Watch(ConfigFileInfo, () =>
            {
                _logger.LogInformation("配置文件改变，重新加载配置文件");
                var changedConfig = Load(config.Path, config.BaraMapper);
                config.BaraMapper.LoadConfig(changedConfig);
            });
            ///监控BaraMap
            foreach (var baraMap in config.BaraMaps)
            {
                var baraMapperFileInfo = FileLoader.GetFileInfo(baraMap.Path);
                FileWatcherLoader.Instance.Watch(baraMapperFileInfo, () =>
                {
                    var baraMapStream = LoadConfigStream(baraMap.Path);
                    var changedBaraMapper = LoadBaraMap(baraMapStream, config);
                    baraMap.Scope = changedBaraMapper.Scope;
                    baraMap.Statements = changedBaraMapper.Statements;

                    config.ClearMappedStatements();

                });
            }
        }

        public ConfigStream LoadConfigStream(String path)
        {
            var configStream = new ConfigStream
            {
                Path = path,
                Stream = FileLoader.Load(path)
            };
            return configStream;
        }

        public void LoadBaraMap(string filePath, BaraMapConfig baraMapConfig)
        {
            var baraMapSteam = LoadConfigStream(filePath);
            var baraMap = LoadBaraMap(baraMapSteam, baraMapConfig);
            baraMapConfig.BaraMaps.Add(baraMap);
        }

        public override void Dispose()
        {
            FileWatcherLoader.Instance.Clear();
        }

    }
}
