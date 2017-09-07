using Bara.Abstract.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Cache
{
    public class LruCacheProvider : ICacheProvider
    {
        public object this[string key, Type type] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public void Initliaze(IDictionary<string, string> dictionary)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
