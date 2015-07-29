using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class CacheEvictionBase<TKey, TValue> : AbstractCacheEviction<TKey, TValue>
    {
        protected CacheEvictionBase(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(owner, capacity)
        {
        }

        protected override Type GetEvictableCollectionType()
        {
            throw new NotImplementedException();
        }

        protected override int GetEvictionSize()
        {
            throw new NotImplementedException();
        }

        protected override bool GetNextEvictedKey(out TKey key)
        {
            throw new NotImplementedException();
        }

        protected override void Before(CacheAccess access, TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        protected override void After(CacheAccess access, TKey key, TValue value)
        {
            throw new NotImplementedException();
        }
    }
}