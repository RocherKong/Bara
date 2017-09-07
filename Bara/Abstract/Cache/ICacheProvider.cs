using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Cache
{
    public interface ICacheProvider
    {
        void Initliaze(IDictionary<String, String> dictionary);

        bool Remove(String key);

        object this[String key, Type type]
        {
            get;
            set;
        }

        void Flush();
    }
}
