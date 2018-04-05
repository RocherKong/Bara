using Bara.Core.Mapper;
using Bara.Model;
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

        internal DynamicParameters DapperDynamicParams { get; set; }

        internal IDictionary<String, object> RequestParameters { get; set; }

        internal BaraMap baraMap { get; set; }

        private object RequestObj { get; set; }

        public object Request
        {
            get { return RequestObj; }
            set
            {
                RequestObj = value;
                if (RequestObj == null)
                {
                    DapperDynamicParams = null;
                    RequestParameters = null;
                    return;
                }
                DapperDynamicParams = new DynamicParameters(RequestObj);
                RequestParameters = new SortedDictionary<String, object>();

                if (RequestObj is IEnumerable<KeyValuePair<String, object>> reqDic)
                {
                    foreach (var kv in reqDic)
                    {
                        RequestParameters.Add(kv.Key, kv.Value);
                    }
                    return;
                }
                var properties = RequestObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in properties)
                {
                    var propertyVal = prop.GetValue(RequestObj);
                    RequestParameters.Add(prop.Name, propertyVal);
                }
            }
        }

        public object Response { get; set; }

        public IDictionary Items { get; set; } = new Dictionary<object, object>();
    }
}
