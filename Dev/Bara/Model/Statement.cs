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
using Bara.Abstract.Cache;
using Bara.Core.Cache;

namespace Bara.Model
{
    public class Statement
    {
        [XmlAttribute]
        public String Id { get; set; }
        public String FullSqlId => $"{BaraMap.Scope}.{Id}";
        public List<ITag> SqlTags { get; set; }

        public Cache Cache { get; set; }

        private ICacheProvider _cacheProvider;

        private static readonly object syncObj = new object();
        public ICacheProvider CacheProvider
        {
            get
            {
                if (_cacheProvider == null)
                {
                    lock (syncObj)
                    {
                        if (_cacheProvider == null)
                        {
                            if (Cache == null)
                            {
                                _cacheProvider = new NoneCacheProvider();
                            }
                            else
                            {
                                _cacheProvider = Cache.CreateCacheProvider(this);
                            }
                        }
                    }
                }
                return _cacheProvider;
            }
        }


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

            var CacheId = xele.Attribute("Cache")?.Value;
            if (!String.IsNullOrEmpty(CacheId))
            {
                var _cache = baraMap.Caches.FirstOrDefault(m => m.Id == CacheId);
                statement.Cache = _cache;
            }

            IEnumerable<XNode> childNodes = xele.Nodes();
            foreach (var node in childNodes)
            {
                String tagName = "";

                if (node.NodeType == System.Xml.XmlNodeType.Element)
                {
                    XElement elenode = node as XElement;

                    tagName = elenode.Name.LocalName;

                    switch (tagName)
                    {
                        case "Include":
                            {
                                var refId = elenode.Attribute("RefId")?.Value;
                                var refStatement = baraMap.Statements.FirstOrDefault(x => x.Id == refId);
                                if (refStatement == null)
                                {
                                    throw new BaraException($"Statement can't Load ref which id is:{refId}");
                                }
                                if (refId == statement.Id)
                                {
                                    throw new BaraException($"Statement which Id :{refId} can't load self,try another Id");
                                }
                                statement.SqlTags.Add(new Include
                                {
                                    RefId = refId,
                                    Ref = refStatement
                                });
                                break;
                            }
                        default:
                            {
                                var tag = LoadTag(node);
                                if (tag != null)
                                {
                                    statement.SqlTags.Add(tag);
                                }
                                break;
                            }
                    }
                }

                if (node.NodeType == System.Xml.XmlNodeType.Text || node.NodeType == System.Xml.XmlNodeType.CDATA)
                {
                    statement.SqlTags.Add(new SqlText
                    {
                        BodyText = node.ToString()
                    });
                }

            }
            return statement;
        }

        public static ITag LoadTag(XNode node)
        {
            ITag tag = null;
            if (node.NodeType == System.Xml.XmlNodeType.Text || node.NodeType == System.Xml.XmlNodeType.CDATA)
            {
                tag = new SqlText
                {
                    BodyText = node.ToString()
                };
            }
            if (node.NodeType == System.Xml.XmlNodeType.Element)
            {
                XElement elenode = node as XElement;
                bool isIn = elenode.Attribute("In") != null;
                var prepend = elenode.Attribute("Prepend")?.Value.Trim();
                var property = elenode.Attribute("Property")?.Value.Trim();
                var compareValue = elenode.Attribute("CompareValue")?.Value.Trim();
                var nodeName = elenode.Name.LocalName;
                switch (nodeName)
                {
                    case "IsEmpty":
                        {
                            tag = new IsEmpty
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                Children = new List<ITag>()
                            };
                            break;
                        }

                    case "IsEqual":
                        {
                            tag = new IsEqual
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                CompareValue = compareValue,
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "IsNotEqual":
                        {
                            tag = new IsNotEqual
                            {
                                In = isIn,
                                Prepend = prepend,
                                CompareValue = compareValue,
                                Property = property,
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "IsGreaterEqual":
                        {
                            tag = new IsGreaterEqual
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                CompareValue = compareValue,
                            };
                            break;
                        }
                    case "IsGreaterThan":
                        {
                            tag = new IsGreaterThan
                            {
                                In = isIn,
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
                                In = isIn,
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
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                CompareValue = compareValue,
                                Children = new List<ITag>()
                            };
                            break;
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
                    case "IsNotNull":
                        {
                            tag = new IsNotNull
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "IsNull":
                        {
                            tag = new IsNull
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "Switch":
                        {
                            tag = new Switch
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "Case":
                        {
                            tag = new Switch.Case
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "Default":
                        {
                            tag = new Switch.Default
                            {
                                In = isIn,
                                Prepend = prepend,
                                Property = property,
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "Where":
                        {
                            tag = new Where
                            {
                                Children = new List<ITag>()
                            };
                            break;
                        }
                    case "Dynamic":
                        {
                            tag = new Dynamic
                            {
                                Prepend = prepend,
                                Children = new List<ITag>()
                            };
                            break;

                        }
                    case "IsProperty":
                        {
                            tag = new IsProperty
                            {
                                Prepend = prepend,
                                Children = new List<ITag>(),
                                In = isIn,
                                Property = property
                            };
                            break;
                        }
                    default:
                        {
                            throw new BaraException($"Statement can't load TagName:{nodeName}");
                        }
                }
                foreach (var childNode in elenode.Nodes())
                {
                    ITag childTag = LoadTag(childNode);
                    if (childTag != null && tag != null)
                    {
                        (tag as Tag).Children.Add(childTag);
                    }
                }
            }
            return tag;
        }
        public String BuildSql(RequestContext context)
        {
            context._baraMap = BaraMap;
            String baraprefix = BaraMap.BaraMapConfig.Settings.ParameterPrefix;
            String dbprefix = BaraMap.BaraMapConfig.DataBase.DbProvider.ParameterPrefix;
            StringBuilder sqlStr = new StringBuilder();
            foreach (ITag tag in SqlTags)
            {
                sqlStr.Append(tag.BuildSql(context));
            }
            return sqlStr.Replace(baraprefix, dbprefix).ToString();
        }
    }
}
