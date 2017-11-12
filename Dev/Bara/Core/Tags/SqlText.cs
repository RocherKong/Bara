using Bara.Abstract.Tag;
using Bara.Core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Tags
{
    public class SqlText : ITag
    {
        public TagType Type => TagType.SqlText;
        public string BodyText { get; set; }
        public string BuildSql(RequestContext context)
        {
            return BodyText;
        }
    }
}
