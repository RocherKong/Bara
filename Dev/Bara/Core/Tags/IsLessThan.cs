using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class IsLessThan : CompareTag
    {
        public override TagType Type => TagType.IsLessThan;

        public override bool IsNeedShow(RequestContext context)
        {
            var reqVal = GetPropertyValue(context);
            bool isNeedShow = false;
            if (!Decimal.TryParse(reqVal.ToString(), out decimal reqValue)) { return false; }
            if (!Decimal.TryParse(CompareValue, out decimal compareValue)) { return false; }
            if (reqValue < compareValue) { return true; }
            return isNeedShow;
        }
    }
}
