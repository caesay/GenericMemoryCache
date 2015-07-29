using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class MemoryCache<TKey, TValue> : MemoryCacheBase<TKey, TValue>
    {
        public MemoryCache()
            : this(DefaultCapacity, DefaultNumberOfWays, null)
        {
        }

        public MemoryCache(int capacity)
            : this(capacity, DefaultNumberOfWays, null)
        {
        }

        public MemoryCache(int capacity, int numberOfWays)
            : this(capacity, numberOfWays, null)
        {
        }

        public MemoryCache(int capacity, int numberOfWays, Func<TKey, int> indexer)
            : base(capacity, numberOfWays, indexer)
        {
        }

        protected override Type GetSubCacheType()
        {
            return typeof(MemorySubCache<TKey, TValue>);
        }
    }
}