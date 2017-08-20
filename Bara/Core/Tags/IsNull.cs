using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Common;

namespace Bara.Core.Tags
{
    public class IsNull : Tag
    {
        public override TagType Type => TagType.IsNull;

        public override bool IsNeedShow(object objParam)
        {
            return objParam.GetValue(Property) == null;
        }
    }
}
