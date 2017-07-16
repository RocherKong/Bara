using Bara.Abstract.Core;
using Bara.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bara.Common
{
    public class LocalConfigLoader
    {
        //总加载器
        //1初始化配置文件（BaraMapConfig），2监控文件BaraMapper   调用器
        public BaraMapConfig Load(String filePath, IBaraMapper baraMapper)
        {
            var Config = LoadConfig(filePath, baraMapper);
            if (Config.Settings.IsWatchConfigFile)
            {
                //监控
                WatchConfig(Config);
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
            }

            return config;
        }

        /// <summary>
        /// 监控配置文件变化
        /// </summary>
        /// <param name="config"></param>
        public void WatchConfig(BaraMapConfig config)
        {
            var ConfigFileInfo = FileLoader.GetFileInfo(config.Path);
            ///监控Config
            FileWatcherLoader.Instance.Watch(ConfigFileInfo, () =>
            {
                var changedConfig = Load(config.Path, config.BaraMapper);
                config.BaraMapper.LoadConfig(changedConfig);
            });
            ///监控BaraMap
            foreach (var baraMap in config.BaraMapSources)
            {
                var baraMapperFileInfo = FileLoader.GetFileInfo(baraMap.Path);
                FileWatcherLoader.Instance.Watch(baraMapperFileInfo, () =>
                {
                    var changedBaraMapper = LoadBaraMap(baraMap.Path, config);

                    config.ClearMappedStatements();

                });
            }
        }

        public BaraMap LoadBaraMap(string filePath, BaraMapConfig baraMapConfig)
        {
            var baraMap = new BaraMap
            {
                Path = filePath,
                Statements = new List<Statement>()
            };

            using (var mapConfigSteam=FileLoader.Load(filePath)) {
                XDocument xdoc = XDocument.Load(mapConfigSteam);
                XElement xele = xdoc.Root;
                XNamespace ns = xele.GetDefaultNamespace();
                IEnumerable<XElement> StatementList = xele.Descendants(ns+"Statement");
                foreach (var statementNode in StatementList)
                {
                    var _statement = Statement.Load(statementNode,baraMap);
                    baraMap.Statements.Add(_statement);
                }
              
            }

            return baraMap;
        }
    }
}
