using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Bara.Common
{
    public static class ObjectExtension
    {
        public static Object GetValue(this Object obj, String PropertyName) {
            //GetRuntimeProperty
            return obj?.GetType().GetRuntimeProperty(PropertyName)?.GetValue(obj);
        }
    }
}
