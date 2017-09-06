using Bara.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bara.Model
{
    public class Cache
    {
        public static Cache Load(XElement element)
        {
            var cache = new Cache
            {
                Id = element.Attribute("Id")?.Value,
                Type = element.Attribute("Type")?.Value,
                Parameters = new Dictionary<String, String>(),
                FlushOnExecutes = new List<CacheFlushOnExecute> { }

            };

            var children = element.Elements();
            foreach (var config in children)
            {
                var tagName = config.Name.LocalName;
                switch (tagName)
                {
                    case "Parameter":
                        {
                            var key = config.Attribute("Key")?.Value;
                            var value = config.Attribute("Value")?.Value;
                            if (!String.IsNullOrEmpty(key))
                            {
                                cache.Parameters.Add(key, value);
                            }
                            break;
                        }
                    case "FlushInterval":
                        {
                            string hours = config.Attribute("Hours")?.Value;
                            string minutes = config.Attribute("Minutes")?.Value;
                            string seconds = config.Attribute("Seconds")?.Value;

                            int.TryParse(hours, out int Hours);
                            int.TryParse(minutes, out int Minutes);
                            int.TryParse(seconds, out int Seconds);
                            cache.FlushInterval = new CacheFlushInterval
                            {
                                Hours = Hours,
                                Minutes = Minutes,
                                Seconds = Seconds
                            };
                            break;

                        }
                    case "FlushOnExecute":
                        {
                            var statementId = config.Attribute("Statement")?.Value;
                            if (!String.IsNullOrEmpty(statementId))
                            {
                                cache.FlushOnExecutes.Add(new CacheFlushOnExecute
                                {
                                    Statement = statementId
                                });
                            }

                            break;

                        }
                    default:
                        {
                            throw new BaraException("无法识别的配置项：" + tagName);
                        }
                }


            }

            return cache;
        }

        public String Id { get; set; }

        public String Type { get; set; }

        public IDictionary<String, String> Parameters { get; set; }

        public class Parameter
        {
            public String Key { get; set; }

            public String Value { get; set; }
        }

        public CacheFlushInterval FlushInterval { get; set; }

        public class CacheFlushInterval
        {
            public TimeSpan Interval => new TimeSpan(Hours, Minutes, Seconds);
            public int Hours { get; set; }

            public int Minutes { get; set; }

            public int Seconds { get; set; }
        }

        public IList<CacheFlushOnExecute> FlushOnExecutes { get; set; }


        public class CacheFlushOnExecute
        {
            public String Statement { get; set; }
        }
    }
}
