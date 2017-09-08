using Bara.Core.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Cache
{
    public interface ICacheProvider
    {
        void Initliaze(IDictionary<CacheKey, String> dictionary);

        bool Remove(CacheKey key);

        object this[CacheKey key, Type type]
        {
            get;
            set;
        }

        void Flush();
    }
}
