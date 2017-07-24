using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Context
{
    /// <summary>
    /// Request Context
    /// Include Request Info
    /// </summary>
    public class RequestContext
    {
        public String SqlId { get; set; }

        public String Scope { get; set; }

        public String FullSqlId { get { return $"{Scope}.{SqlId}"; } }

        public object Request { get; set; }

        public object Response { get; set; }

        public IDictionary Items { get; set; } = new Dictionary<object, object>();
    }
}
