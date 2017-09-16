using Bara.Abstract.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Bara.Core.Cache
{
    public class NoneCacheProvider : ICacheProvider
    {
        public object this[CacheKey key, Type type] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Flush()
        {
           // throw new NotImplementedException();
        }

        public void Initliaze(IDictionary dictionary)
        {
          //  throw new NotImplementedException();
        }

        public bool Remove(CacheKey key)
        {
            throw new NotImplementedException();
        }
    }
}
