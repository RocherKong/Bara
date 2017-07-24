using Bara.Abstract.Tag;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Core.Context;
using Bara.Model;

namespace Bara.Core.Tags
{
    public class Include : ITag
    {
        public TagType MyProperty { get; set; }

        public TagType Type => TagType.Include;

        public Statement Ref { get; set; }

        public string BuildSql(RequestContext context, string parameterPrefix)
        {
            return Ref.BuildSql(context);
        }
    }
}
