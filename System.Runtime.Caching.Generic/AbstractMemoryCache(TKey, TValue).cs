using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public abstract class AbstractMemoryCache<TKey, TValue> : AbstractEvictingCache<TKey, TValue>, IMemoryCache<TKey, TValue>
    {
        protected AbstractMemoryCache(int capacity, int numberOfWays, Func<TKey, int> indexer)
            : base(capacity)
        {
            if (numberOfWays <= 0)
            {
                throw new ArgumentOutOfRangeException("numberOfWays", "must be strictly greater than zero");
            }
            SubCacheCapacity = capacity / numberOfWays;
            NumberOfWays = numberOfWays;
            Indexer = indexer ?? DefaultIndexer;
        }

        protected abstract Type GetSubCacheType();

        protected virtual int DefaultIndexer(TKey key)
        {
            return Math.Abs(key.GetHashCode() % NumberOfWays);
        }
        
        protected int SubCacheCapacity { get; private set; }

        protected int NumberOfWays { get; private set; }

        #region IMemoryCache<TKey, TValue> implementation
        public Func<TKey, int> Indexer { get; private set; }
        #endregion
    }
}