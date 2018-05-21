using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class IsEqual : CompareTag
    {
        public override TagType Type => TagType.IsEqual;

        public override bool IsNeedShow(RequestContext context)
        {
            var reqVal = GetPropertyValue(context);
            bool isNeedShow = false;
            if (!decimal.TryParse(CompareValue, out decimal compareValue)) { return false; }
            if (!decimal.TryParse(reqVal.ToString(), out decimal reqValue)) { return false; }
            if (compareValue == reqValue)
            {
                return true;
            }
            return isNeedShow;

        }
    }
}
