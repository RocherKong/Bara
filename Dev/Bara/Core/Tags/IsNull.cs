using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class IsNull : Tag
    {
        public override TagType Type => TagType.IsNull;

        public override bool IsNeedShow(RequestContext context)
        {
            return GetPropertyValue(context) == null;
        }
    }
}
