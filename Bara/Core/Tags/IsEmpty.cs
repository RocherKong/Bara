using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;

namespace Bara.Core.Tags
{
    public class IsEmpty : Tag
    {
        public override TagType Type => TagType.IsEmpty;

        public override bool IsNeedShow(object objParam)
        {
            var reqVal = objParam.GetValue(Property);
            return !(reqVal != null && reqVal.ToString().Length > 0);
        }
    }
}
