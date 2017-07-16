using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Bara.Model
{
    [XmlRoot()]
    public class BaraMap
    {
        [XmlAttribute("Scope")]
        public String Scope { get; set; }

        //public List<Cache> Caches { get; set; }
        [XmlArray]
        public IList<Statement> Statements { get; set; }

        [XmlIgnore]
        public String Path { get; set; }
    }
}
