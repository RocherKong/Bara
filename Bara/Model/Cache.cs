using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Bara.Model
{
    public class Cache
    {
        [XmlAttribute]
        public String Id { get; set; }

        [XmlAttribute]
        public String Type { get; set; }

        [XmlArray("Parameter")]
        public List<Parameter> Parameters { get; set; }


        public class Parameter
        {
            public String Key { get; set; }

            public String Value { get; set; }
        }

        [XmlElement("FlushInterval")]
        public FlushInterval flushInterval { get; set; }

        public class FlushInterval
        {
            public int Hours { get; set; }

            public int Minutes { get; set; }

            public int Seconds { get; set; }
        }

        [XmlArray("FlushOnExecute")]
        public List<FlushOnExecute> flushOnExecute { get; set; }

        public class FlushOnExecute
        {
            public String Statement { get; set; }
        }
    }
}
