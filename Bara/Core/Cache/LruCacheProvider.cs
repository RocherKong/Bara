using Bara.Abstract.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Cache
{
    public class LruCacheProvider : ICacheProvider
    {
        public Hashtable _ht;
        public LruCacheProvider()
        {
            _ht = Hashtable.Synchronized(new Hashtable());
        }
        public object this[CacheKey key, Type type]
        {
            get { return _ht[key]; }
            set { _ht[key] = type; }
        }

        public void Flush()
        {
            _ht.Clear();
        }

        public void Initliaze(IDictionary<CacheKey, string> dictionary)
        {
            throw new NotImplementedException();
        }

        public bool Remove(CacheKey key)
        {
            _ht.Remove(key);
            return true;
        }
    }
}
