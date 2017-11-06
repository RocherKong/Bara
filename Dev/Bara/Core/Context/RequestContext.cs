using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

        public DynamicParameters RequestDynamicParams { get; set; }

        public object RequestObj { get; set; }

        public object Request
        {
            get { return RequestObj; }
            set
            {
                RequestObj = value;
                if (RequestObj == null) { return; }
                RequestDynamicParams = new DynamicParameters();
                if ((RequestObj is DynamicParameters) || (RequestObj is IEnumerable<KeyValuePair<String, object>>))
                {
                    RequestDynamicParams.AddDynamicParams(RequestObj);
                }
                else
                {
                    var properties = RequestObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in properties)
                    {
                        var propertyVal = prop.GetValue(RequestObj);
                        RequestDynamicParams.Add(prop.Name, propertyVal);
                    }
                }
            }
        }

        public object Response { get; set; }

        public IDictionary Items { get; set; } = new Dictionary<object, object>();
    }
}
