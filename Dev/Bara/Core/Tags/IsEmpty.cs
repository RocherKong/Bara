using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;
using System.Collections;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class IsEmpty : Tag
    {
        public override TagType Type => TagType.IsEmpty;

        public override bool IsNeedShow(RequestContext context)
        {
            var reqVal = GetPropertyValue(context);
            if (reqVal is IEnumerable reqArray)
            {
                return !reqArray.GetEnumerator().MoveNext();
            }
            return !(reqVal != null && reqVal.ToString().Length > 0);
        }
    }
}
