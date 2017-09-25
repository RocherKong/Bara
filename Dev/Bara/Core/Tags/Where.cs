using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    public class Where : Dynamic
    {
        public override TagType Type => TagType.Where;

        public override String Prepend { get { return "Where"; } }

        public override string BuildSql(RequestContext context, string parameterPrefix)
        {
            return base.BuildChildSql(context, parameterPrefix);
        }
    }
}
