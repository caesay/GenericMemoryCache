using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    using Collections;

    public class MruCacheEviction<TKey, TValue> : FifoCacheEviction<TKey, TValue>
    {
        public MruCacheEviction(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(owner, capacity)
        {
        }

        protected override bool GetNextEvictedKey(out TKey key)
        {
            key = ((IDoubleEndedUniqueCollection<TKey>)Evictables).Last;
            return true;
        }
    }
}