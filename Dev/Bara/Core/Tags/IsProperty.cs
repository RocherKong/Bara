using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using System.Reflection;
using Bara.Core.Context;

namespace Bara.Core.Tags
{
    /// <summary>
    /// Use for judge Property Exists.
    /// </summary>
    public class IsProperty : Tag
    {
        public override TagType Type => TagType.IsProperty;

        public override bool IsNeedShow(RequestContext context)
        {
            if (context.RequestParameters == null) { return false; }
            return context.RequestParameters.ContainsKey(Property);
        }
    }
}
