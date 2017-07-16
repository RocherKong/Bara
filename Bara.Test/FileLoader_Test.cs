using Bara.Common;
using Bara.Model;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using Xunit;
using System.Xml.Serialization;
using System.Xml;
using Bara.Abstract.Core;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Bara.Test
{
    public class FileLoader_Test
    {
        [Fact]
        public void FileLoaderTest()
        {
            var fileInfo = FileLoader.GetFileInfo(@"E:\BaraMapConfig.xml");
            var fileStream = FileLoader.Load(@"E:\BaraMapConfig.xml");
            Trace.WriteLine("ok");
        }

        [Fact]
        public void FileWatchTest()
        {
            int ChangeTimes = 0;
            var fileInfo = FileLoader.GetFileInfo(@"E:\BaraMapConfig.xml");
            FileWatcherLoader.Instance.Watch(fileInfo, () =>
            {
                Trace.WriteLine("File Change Times:" + ChangeTimes);
                ChangeTimes++;
            });

            FileWatcherLoader.Instance.Clear();
            Thread.Sleep(10000);
            Trace.WriteLine("Test OK");
        }

        [Fact]
        public void ConfigDeserizeTest()
        {
            //var fileInfo = FileLoader.GetFileInfo(@"E:\BaraMapConfig.xml");
            //var fileStream = FileLoader.Load(@"E:\BaraMapConfig.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(BaraMapConfig));
            BaraMapConfig config = null;
            using (var configStream = FileLoader.Load(@"E:\BaraMapConfig.xml"))
            {
                config = serializer.Deserialize(configStream) as BaraMapConfig;

            }

            foreach (var baramap in config.BaraMapSources)
            {

            }

            Trace.WriteLine("OK");
            //  BaraMapConfig config = JsonConvert.DeserializeObject<BaraMapConfig>(fileStream);
        }

        [Fact]
        public void LocalConfigLoaderTest()
        {
            LocalConfigLoader loader = new LocalConfigLoader();

            // IBaraMapper baraMapper;
            var config = loader.Load("BaraMapConfig.xml", null);

        }
        [Fact]
        public void XmlLoaderTest()
        {
            XDocument doc = XDocument.Load(@"E:\T_Test.xml");
            XElement xele = doc.Root;
            XNamespace ns = xele.GetDefaultNamespace();
            //XElement xStatements =new XElement(ns+"Statements");
            foreach (var item in xele.Descendants(ns+"Statement"))
            {
                Trace.WriteLine(item.Name);
            }


            //IEnumerable<XElement> elements = from ele in doc.Descendants()
            //                                 select ele;

            //var Scope = elements.Attributes("Scope");
            //IEnumerable<XElement> Statements = xele.Elements();
            //foreach (var item in Statements)
            //{
            //    Trace.WriteLine(item.Name);
            //}

          //  Trace.WriteLine(Scope);

        }
    }
}
