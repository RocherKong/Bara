using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Bara.Common
{
    public static class ObjectExtension
    {
        public static Object GetValue(this Object obj, String PropertyName)
        {
            var dynamicParams = obj as Dapper.DynamicParameters;
            if (dynamicParams != null)
            {
                return dynamicParams.Get<object>(PropertyName);
            }
            else
            {
                return obj?.GetType().GetRuntimeProperty(PropertyName)?.GetValue(obj);
            }
        }
    }
}
