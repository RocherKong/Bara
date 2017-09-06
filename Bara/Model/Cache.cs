using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bara.Model
{
    public class Cache
    {
        public static Cache Load(XElement element) {
            var cache = new Cache {
                Id = element.Attribute("Id").Value,
                Type = element.Attribute("Type").Value,
                Parameters = new Dictionary<String, String>(),
                FlushOnExecutes = new List<CacheFlushOnExecute> { }
                
            };



            return cache;
        }

        public String Id { get; set; }

        public String Type { get; set; }

        public IDictionary<String,String> Parameters { get; set; }

        public class Parameter
        {
            public String Key { get; set; }

            public String Value { get; set; }
        }

        public CacheFlushInterval FlushInterval { get; set; }

        public class CacheFlushInterval
        {
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
