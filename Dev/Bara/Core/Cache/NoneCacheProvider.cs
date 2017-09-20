using Bara.Abstract.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Bara.Core.Cache
{
    public class NoneCacheProvider : ICacheProvider
    {
        public object this[CacheKey key, Type type] { get { return null; } set => throw new NotImplementedException(); }

        public void Flush()
        {
            return;
        }

        public void Initliaze(IDictionary dictionary)
        {
            return;
        }

        public bool Remove(CacheKey key)
        {
            return true;
        }
    }
}
