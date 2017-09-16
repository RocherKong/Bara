using Bara.Abstract.Tag;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Core.Context;
using Bara.Common;

namespace Bara.Core.Tags
{
    public class IsNotEmpty : Tag
    {
        public override TagType Type => TagType.IsNotEmpty;

        public override bool IsNeedShow(object objParam)
        {
            var reqVal = objParam.GetValue(Property);
            return reqVal != null && reqVal.ToString().Length > 0;
        }
    }
}
