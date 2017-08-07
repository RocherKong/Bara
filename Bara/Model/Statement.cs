using Bara.Abstract.Tag;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using Bara.Exceptions;
using Bara.Core.Context;
using Bara.Core.Tags;

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
                    case "Include":
                        {
                            var refId = childNode.Attribute("RefId").Value;
                            var refStatement = baraMap.Statements.FirstOrDefault(x => x.Id == refId);
                            if (refStatement == null)
                            {
                                throw new BaraException($"Bara.Statement Can't find Statement which Id is {refId}");
                            }
                            if (refId == Statement.Id)
                            {
                                throw new BaraException($"Bara.Statement Can't Use Confict StatementId");
                            }
                            Statement.SqlTags.Add(new Include
                            {
                                RefId=refId,
                                Ref=refStatement
                            });
                            break;
                        }
                    default:
                        {
                            var tag = LoadTag(childNode);
                            if (tag != null) {
                                Statement.SqlTags.Add(tag);
                            }
                            break;
                        };
                }

            }




            return Statement;
        }


        public static ITag LoadTag(XElement xmlNode) {
            ITag tag = null;
            bool isIn = xmlNode.Attributes("In") != null;
            var prepend = xmlNode.Attribute("Prepend")?.Value;
            var property = xmlNode.Attribute("Property")?.Value;
            var compareValue = xmlNode.Attribute("CompareValue")?.Value;
            switch (xmlNode.Name.LocalName)
            {
                
                case "#text":
                case "cdata-section": {
                        var bodyText = xmlNode.Value.Replace("\n", "");
                        return new SqlText
                        {
                            BodyText = bodyText
                        };
                    }
                case "IsNotEmpty": {
                        tag = new IsNotEmpty
                        {
                            In = isIn,
                            Prepend = prepend,
                            Property = property,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                default:
                    return null;
            }

            foreach (var Child in xmlNode.Descendants())
            {
                ITag childtag = LoadTag(Child);
                (tag as Tag).Children.Add(childtag);
                
            }

            return tag;


        }

        public String BuildSql(RequestContext context)
        {
            String prefix = BaraMap.BaraMapConfig.DataBase.DbProvider.ParameterPrefix;
            StringBuilder sqlStr = new StringBuilder();
            foreach (ITag tag in SqlTags)
            {
                sqlStr.Append(tag.BuildSql(context, prefix));
            }
            return sqlStr.ToString();
        }

    }
}
