using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public sealed class NoCacheEviction<TKey, TValue> : CacheEvictionBase<TKey, TValue>
    {
        public NoCacheEviction(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(owner, capacity)
        {
        }

        protected override Type GetEvictableCollectionType()
        {
            return null;
        }
    }
}