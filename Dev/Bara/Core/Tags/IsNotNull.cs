using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;

namespace Bara.Core.Tags
{
    public class IsNotNull : Tag
    {
        public override TagType Type => TagType.IsNotNull;

        public override bool IsNeedShow(object objParam)
        {
            var reqVal = objParam.GetValue(Property);
            bool IsNeedShow = !(reqVal==null);
            return IsNeedShow;
        }
    }
}
