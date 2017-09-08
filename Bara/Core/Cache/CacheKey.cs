using Bara.Core.Context;
using System;
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
                if (RequestContext.Request == null)
                {
                    return "Null";
                }
                var properties = RequestContext.Request.GetType().GetProperties().OrderBy(p => p.Name);
                StringBuilder sb = new StringBuilder();
                foreach (var prop in properties)
                {
                    sb.AppendFormat("&{0}",prop.GetValue(RequestContext.Request));
                }
                return sb.ToString().Trim('&');
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
            if (obj == this) {
                return true;
            }
            if (!(obj is CacheKey)) {
                return false;
            }
            return (obj as CacheKey).Key == Key;

        }
    }
}
