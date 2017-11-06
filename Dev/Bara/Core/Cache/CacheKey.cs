using Bara.Core.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bara.Core.Cache
{
    public class CacheKey
    {
        public RequestContext RequestContext { get; set; }

        public String RequestContextString
        {
            get
            {
                if (RequestContext.RequestDynamicParams == null)
                {
                    return "Null";
                }
                var reqParams = RequestContext.RequestDynamicParams;
                var properties = reqParams.ParameterNames.ToList().OrderBy(x => x);

                StringBuilder sb = new StringBuilder();
                foreach (var prop in properties)
                {
                    var val = reqParams.Get<object>(prop);
                    BuildSqlQueryString(sb, prop, val);

                }
                return sb.ToString().Trim('&');
            }
        }

        private void BuildSqlQueryString(StringBuilder strBuilder, String key, object val)
        {
            if (val is IEnumerable list && !(val is String))
            {
                strBuilder.AppendFormat("&{0}=(", key);
                foreach (var item in list)
                {
                    strBuilder.AppendFormat("{0}", item);
                }
                strBuilder.Append(")");
            }
            else
            {
                strBuilder.AppendFormat("&{0}={1}", key, val);
            }
        }
        public String Key { get { return $"{RequestContext.FullSqlId}:{RequestContextString}"; } }
        public CacheKey(RequestContext context)
        {
            this.RequestContext = context;
        }

        public override string ToString()
        {
            return Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (!(obj is CacheKey))
            {
                return false;
            }
            return (obj as CacheKey).Key == Key;

        }
    }
}
