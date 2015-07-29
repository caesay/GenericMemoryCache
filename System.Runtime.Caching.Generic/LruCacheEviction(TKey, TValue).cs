using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    using Collections;

    public class LruCacheEviction<TKey, TValue> : FifoCacheEviction<TKey, TValue>
    {
        public LruCacheEviction(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(owner, capacity)
        {
        }

        protected override bool GetNextEvictedKey(out TKey key)
        {
            key = ((IDoubleEndedUniqueCollection<TKey>)Evictables).First;
            return true;
        }
    }
}