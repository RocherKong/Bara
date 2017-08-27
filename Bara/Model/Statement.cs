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

        public String FullSqlId => $"{BaraMap.Scope}.{Id}";

        public List<ITag> SqlTags { get; set; }

        [XmlIgnore]
        public BaraMap BaraMap { get; private set; }

        public static Statement Load(XElement xele, BaraMap baraMap)
        {
            var statement = new Statement
            {
                Id = xele.Attribute("Id").Value,
                SqlTags = new List<ITag> { },
                BaraMap = baraMap
            };

            statement.SqlTags.AddRange(GetTagList(xele,baraMap));
            //IEnumerable<XNode> childNodes = xele.Nodes();
            //foreach (var node in childNodes)
            //{
            //    var tag = LoadEle(node, baraMap);
            //    if (tag != null)
            //        statement.SqlTags.Add(tag);
            //}

            return statement;


        }

        public static IList<ITag> GetTagList(XElement xele, BaraMap baraMap)
        {
            IList<ITag> tagList = new List<ITag>();
            IEnumerable<XNode> childNodes = xele.Nodes();
            foreach (var node in childNodes)
            {
                var tag = LoadEle(node, baraMap);
                if (tag != null)
                    tagList.Add(tag);
            }
            return tagList;
        }

        public static ITag LoadEle(XNode xnode, BaraMap baraMap)
        {
            if (xnode.NodeType == System.Xml.XmlNodeType.Text)
            {
                return new SqlText
                {
                    BodyText = xnode.ToString(),
                };
            }

            if (xnode.NodeType == System.Xml.XmlNodeType.Comment)
            {
                return null;
            }


            //如果是正常Statement 加载不同tag
            var ele = (xnode as XElement);
            var tagName = ele.Name.LocalName;
            switch (tagName)
            {
                case "Include":
                    {
                        var refId = ele.Attribute("RefId").Value;
                        return new Include
                        {
                            RefId = refId,
                            Ref = baraMap.Statements.FirstOrDefault(x => x.Id == refId)
                        };
                    }
                default:
                    {
                        ITag tag;
                        bool isIn = ele?.Attributes("In") != null;
                        var prepend = ele?.Attribute("Prepend")?.Value;
                        var property = ele?.Attribute("Property")?.Value;
                        var compareValue = ele?.Attribute("CompareValue")?.Value;
                        switch (ele?.Name.LocalName)
                        {
                            case "#text":
                            case "cdata-section":
                                {
                                    var bodyText = ele.Value.Replace("\n", "");
                                    return new SqlText
                                    {
                                        BodyText = bodyText
                                    };
                                }
                            case "IsNotEmpty":
                                {
                                    tag = new IsNotEmpty
                                    {
                                        In = isIn,
                                        Prepend = prepend,
                                        Property = property,
                                        Children = new List<ITag>()
                                    };
                                    break;
                                }
                            case "IsEmpty":
                                {
                                    tag = new IsEmpty
                                    {
                                        Property = property,
                                        Prepend = prepend,
                                        Children = new List<ITag>()
                                    };
                                    break;
                                }
                            case "IsGreaterThan":
                                {
                                    tag = new IsGreaterThan
                                    {
                                        Prepend = prepend,
                                        Property = property,
                                        CompareValue = compareValue,
                                        Children = new List<ITag>()
                                    };
                                    break;
                                }
                            case "IsGreaterEqual":
                                {
                                    tag = new IsGreaterEqual
                                    {
                                        Prepend = prepend,
                                        Property = property,
                                        CompareValue = compareValue,
                                        Children = new List<ITag>()
                                    };
                                    break;
                                }
                            case "IsLessThan":
                                {
                                    tag = new IsLessThan
                                    {
                                        Prepend = prepend,
                                        Property = property,
                                        CompareValue = compareValue,
                                        Children = new List<ITag>()
                                    };
                                    break;
                                }
                            case "IsLessEqual":
                                {
                                    tag = new IsLessEqual
                                    {
                                        Prepend = prepend,
                                        Property = property,
                                        CompareValue = compareValue,
                                        Children = new List<ITag>()
                                    };
                                    break;
                                }
                            case "IsEqual":
                                {
                                    tag = new IsEqual
                                    {
                                        Prepend = prepend,
                                        Property = property,
                                        CompareValue = compareValue,
                                        Children = new List<ITag>()
                                    };
                                    break;
                                }
                            default:
                                return null;
                        };

                        if (ele.Nodes().Count() > 0)
                        {
                            //var taglist=GetTagList(ele, baraMap);
                            //foreach (var _tag in taglist)
                            //{

                            //}
                            //(tag as Tag).Children.Add(LoadEle((ele as XNode), baraMap));
                        }
                        return tag;
                    }

            }


            //IEnumerable<XNode> childnodes = ele.Nodes();
            //foreach (var node in childnodes)
            //{
            //    switch (node.NodeType)
            //    {
            //        case System.Xml.XmlNodeType.Attribute:
            //            break;
            //        case System.Xml.XmlNodeType.CDATA:
            //            break;
            //        case System.Xml.XmlNodeType.Comment:
            //            break;
            //        case System.Xml.XmlNodeType.Document:
            //            break;
            //        case System.Xml.XmlNodeType.DocumentFragment:
            //            break;
            //        case System.Xml.XmlNodeType.DocumentType:
            //            break;
            //        case System.Xml.XmlNodeType.Element:
            //            {
            //                return LoadEle(node as XElement,baraMap);
            //            };
            //        case System.Xml.XmlNodeType.EndElement:
            //            break;
            //        case System.Xml.XmlNodeType.EndEntity:
            //            break;
            //        case System.Xml.XmlNodeType.Entity:
            //            break;
            //        case System.Xml.XmlNodeType.EntityReference:
            //            break;
            //        case System.Xml.XmlNodeType.None:
            //            break;
            //        case System.Xml.XmlNodeType.Notation:
            //            break;
            //        case System.Xml.XmlNodeType.ProcessingInstruction:
            //            break;
            //        case System.Xml.XmlNodeType.SignificantWhitespace:
            //            break;
            //        case System.Xml.XmlNodeType.Text:
            //            {
            //                return new SqlText
            //                {
            //                    BodyText = node.ToString(),
            //                };

            //            };
            //        case System.Xml.XmlNodeType.Whitespace:
            //            break;
            //        case System.Xml.XmlNodeType.XmlDeclaration:
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //return null;
        }

        //public static Statement Load(XElement xele, BaraMap baraMap)
        //{
        //    var Statement = new Statement
        //    {
        //        Id = xele.Attribute("Id").Value,
        //        SqlTags = new List<ITag> { },
        //        BaraMap = baraMap
        //    };
        //    XNamespace ns = xele.GetDefaultNamespace();
        //    ///tag
        //    IEnumerable<XElement> childNodes = xele.Elements();

        //    foreach (var childNode in childNodes)
        //    {
        //        var prepend = childNode?.Attribute("Prepend")?.Value;
        //        var property = childNode?.Attribute("Property")?.Value;
        //        var NodeName = childNode?.Name.LocalName.ToString();
        //        switch (NodeName)
        //        {
        //            case "Include":
        //                {
        //                    var refId = (childNode as XElement).Attribute("RefId").Value;
        //                    var refStatement = baraMap.Statements.FirstOrDefault(x => x.Id == refId);
        //                    if (refStatement == null)
        //                    {
        //                        throw new BaraException($"Bara.Statement Can't find Statement which Id is {refId}");
        //                    }
        //                    if (refId == Statement.Id)
        //                    {
        //                        throw new BaraException($"Bara.Statement Can't Use Confict StatementId");
        //                    }
        //                    Statement.SqlTags.Add(new Include
        //                    {
        //                        RefId = refId,
        //                        Ref = refStatement
        //                    });
        //                    break;
        //                }
        //            default:
        //                {
        //                    var tag = LoadTag((childNode as XElement));
        //                    if (tag != null)
        //                    {
        //                        Statement.SqlTags.Add(tag);
        //                    }
        //                    break;
        //                };
        //        }

        //    }




        //    return Statement;
        //}


        public static ITag LoadTag(XElement xmlNode)
        {
            ITag tag = null;
            bool isIn = xmlNode?.Attributes("In") != null;
            var prepend = xmlNode?.Attribute("Prepend")?.Value;
            var property = xmlNode?.Attribute("Property")?.Value;
            var compareValue = xmlNode?.Attribute("CompareValue")?.Value;
            switch (xmlNode?.Name.LocalName)
            {
                case "#text":
                case "cdata-section":
                    {
                        var bodyText = xmlNode.Value.Replace("\n", "");
                        return new SqlText
                        {
                            BodyText = bodyText
                        };
                    }
                case "IsNotEmpty":
                    {
                        tag = new IsNotEmpty
                        {
                            In = isIn,
                            Prepend = prepend,
                            Property = property,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                case "IsEmpty":
                    {
                        tag = new IsEmpty
                        {
                            Property = property,
                            Prepend = prepend,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                case "IsGreaterThan":
                    {
                        tag = new IsGreaterThan
                        {
                            Prepend = prepend,
                            Property = property,
                            CompareValue = compareValue,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                case "IsGreaterEqual":
                    {
                        tag = new IsGreaterEqual
                        {
                            Prepend = prepend,
                            Property = property,
                            CompareValue = compareValue,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                case "IsLessThan":
                    {
                        tag = new IsLessThan
                        {
                            Prepend = prepend,
                            Property = property,
                            CompareValue = compareValue,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                case "IsLessEqual":
                    {
                        tag = new IsLessEqual
                        {
                            Prepend = prepend,
                            Property = property,
                            CompareValue = compareValue,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                case "IsEqual":
                    {
                        tag = new IsEqual
                        {
                            Prepend = prepend,
                            Property = property,
                            CompareValue = compareValue,
                            Children = new List<ITag>()
                        };
                        break;
                    }
                default:
                    return null;
            }

            foreach (var Child in xmlNode.Nodes())
            {
                if (Child.NodeType == System.Xml.XmlNodeType.Text)
                {
                    (tag as Tag).Children.Add(new SqlText
                    {
                        BodyText = Child.ToString()
                    });
                }
                else
                {
                    ITag childtag = LoadTag(Child as XElement);
                    (tag as Tag).Children.Add(childtag);
                }


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
