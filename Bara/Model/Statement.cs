using Bara.Abstract.Tag;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bara.Model
{
    public class Statement
    {
        [XmlAttribute]
        public String Id { get; set; }

        public List<ITag> SqlTags { get; set; }

        public BaraMap BaraMap { get; set; }

        public static Statement Load(XElement xele, BaraMap baraMap)
        {
            var Statement = new Statement
            {
                Id = xele.Attribute("Id").Value,
                SqlTags = new List<ITag> { },
                BaraMap = baraMap
            };
            XNamespace ns = xele.GetDefaultNamespace();

            ///tag
            IEnumerable<XElement> childNodes = xele.Descendants();
            foreach (var childNode in childNodes)
            {
                var prepend = childNode.Attribute("Prepend").Value;
                var property = childNode.Attribute("Property").Value;
                var NodeName = childNode.Name.ToString();
                switch (NodeName)
                {
                    case "IsNotEmpty":
                        {

                            break;
                        }
                    default:
                        break;
                }

            }




            return Statement;
        }

    }
}
