using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    using Collections;

    public class FifoCacheEviction<TKey, TValue> : CacheEvictionBase<TKey, TValue>
    {
        public FifoCacheEviction(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(owner, capacity)
        {
        }

        protected override Type GetEvictableCollectionType()
        {
            return typeof(DoubleEndedUniqueCollection<TKey>);
        }

        protected override int GetEvictionSize()
        {
            return 1;
        }

        protected override void Before(CacheAccess access, TKey key, TValue value)
        {
            if (access == CacheAccess.Get)
            {
                // On cache hits, the LRU/MRU policies consist in moving
                // the hit item keys to the end of the evictable collection
                // (see also After(...) below)
                Evictables.Remove(key);
            }
        }

        protected override void After(CacheAccess access, TKey key, TValue value)
        {
            Evictables.Add(key);
        }
    }
}