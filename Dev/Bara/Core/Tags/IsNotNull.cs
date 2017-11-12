using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class IsNotNull : Tag
    {
        public override TagType Type => TagType.IsNotNull;

        public override bool IsNeedShow(RequestContext context)
        {
            var reqVal = context.GetValue(Property);
            bool IsNeedShow = !(reqVal==null);
            return IsNeedShow;
        }
    }
}
