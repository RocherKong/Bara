using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Tag;
using System.Reflection;

namespace Bara.Core.Tags
{
    /// <summary>
    /// Use for judge Property Exists.
    /// </summary>
    public class IsProperty : Tag
    {
        public override TagType Type => TagType.IsProperty;

        public override bool IsNeedShow(object objParam)
        {
            return objParam.GetType().GetProperty(Property) != null;
        }
    }
}
