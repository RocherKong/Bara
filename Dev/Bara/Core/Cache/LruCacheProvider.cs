using Bara.Abstract.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Cache
{
    public class LruCacheProvider : ICacheProvider
    {
        private int _cacheSize = 0;

        private Hashtable _ht = null;

        private IList _keyList = null;
        public LruCacheProvider()
        {
            _cacheSize = 100;
            _ht = Hashtable.Synchronized(new Hashtable());
            _keyList = ArrayList.Synchronized(new ArrayList());
        }
        public object this[CacheKey key, Type type]
        {
            get
            {
                _keyList.Remove(key);
                _keyList.Add(key);
                return _ht[key];
            }
            set
            {
                _ht[key] = value;
                _keyList.Add(key);
                if (_keyList.Count > _cacheSize)
                {
                    object _key = _keyList[0];
                    _keyList.RemoveAt(0);
                    _ht.Remove(_key);
                }
            }
        }

        public void Flush()
        {
            _ht.Clear();
        }

        public void Initliaze(IDictionary dictionary)
        {
            string size = dictionary["CacheSize"].ToString();
            if (size != null)
            {
                _cacheSize = Convert.ToInt32(size);
            }
        }

        public bool Remove(CacheKey key)
        {
            _ht.Remove(key);
            return true;
        }
    }
}
