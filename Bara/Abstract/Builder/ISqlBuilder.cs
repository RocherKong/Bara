using Bara.Core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Builder
{
    public interface ISqlBuilder
    {
        String BuildSql(RequestContext context);
    }
}
