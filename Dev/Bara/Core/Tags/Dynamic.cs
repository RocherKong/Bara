using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class Dynamic : Tag
    {
        public override TagType Type => TagType.Dynamic;

        public override bool IsNeedShow(RequestContext context)
        {
            return true;
        }

        public override string BuildSql(RequestContext context)
        {
            return BuildChildSql(context);
        }

        public String BuildChildSql(RequestContext context)
        {
            StringBuilder strSql = new StringBuilder();
            if (Children != null && Children.Count > 0)
            {
                bool isFirstTag = true;
                foreach (var childtag in Children)
                {
                    var _sql = childtag.BuildSql(context);
                    if (String.IsNullOrEmpty(_sql))
                    {
                        continue;
                    }

                    ///If First Tag Need Remove Prepend String.
                    if (isFirstTag)
                    {
                        if (!(childtag is SqlText))
                        {
                            Tag tag = childtag as Tag;
                            _sql = _sql.TrimStart();
                            if (!String.IsNullOrEmpty(tag.Prepend))
                            {
                                String _prepend = tag.Prepend.TrimStart();
                                _sql = _sql.Substring(_prepend.Length);
                            }
                        }
                        _sql = $"{Prepend}{_sql}";
                        isFirstTag = false;
                    }
                    strSql.Append(_sql);
                }
            }
            return strSql.ToString();
        }
    }
}
