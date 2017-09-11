using Bara.Core.Cache;
using System;
using System.Collections;
using System.Text;

namespace Bara.Abstract.Cache
{
    public interface ICacheProvider
    {
        void Initliaze(IDictionary dictionary);

        bool Remove(CacheKey key);

        object this[CacheKey key, Type type]
        {
            get;
            set;
        }

        void Flush();
    }
}
