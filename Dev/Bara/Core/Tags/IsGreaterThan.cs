using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class IsGreaterThan : CompareTag
    {
        public override TagType Type => TagType.IsGreaterThan;

        public override bool IsNeedShow(RequestContext context)
        {
            var reqVal = context.GetValue(Property);
            bool isNeedShow = false;
            if (reqVal == null) return false;
            if (!Decimal.TryParse(reqVal.ToString(), out decimal reqValue)) { return false; }
            if (!Decimal.TryParse(CompareValue.ToString(), out decimal compareValue)) { return false; }
            if (reqValue > compareValue) { return true; }
            return isNeedShow;

        }
    }
}
