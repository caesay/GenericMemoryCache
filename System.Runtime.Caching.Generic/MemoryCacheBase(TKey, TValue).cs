using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class MemoryCacheBase<TKey, TValue> : AbstractMemoryCache<TKey, TValue>
    {
        private IEvictingCache<TKey, TValue>[] subCaches;

        protected MemoryCacheBase()
            : this(DefaultCapacity, DefaultNumberOfWays, null)
        {
        }

        protected MemoryCacheBase(int capacity)
            : this(capacity, DefaultNumberOfWays, null)
        {
        }

        protected MemoryCacheBase(int capacity, int numberOfWays)
            : this(capacity, numberOfWays, null)
        {
        }

        protected MemoryCacheBase(int capacity, int numberOfWays, Func<TKey, int> indexer)
            : base(capacity, numberOfWays, indexer)
        {
            subCaches = new IEvictingCache<TKey, TValue>[numberOfWays];
            for (var i = 0; i < numberOfWays; i++)
            {
                subCaches[i] = (IEvictingCache<TKey, TValue>)Activator.CreateInstance(GetSubCacheType(), this, SubCacheCapacity);
            }
            Policy = null;
        }

        protected override Type GetSubCacheType()
        {
            throw new NotImplementedException();
        }

        protected IManagedCache<TKey, TValue> GetSubCache(TKey key)
        {
            return subCaches[NumberOfWays > 1 ? Indexer(key) : 0];
        }

        protected override void Prepare(ICachePolicy<TKey, TValue> policy)
        {
            foreach (var subCache in subCaches)
            {
                subCache.Policy = policy;
            }
        }

        protected override IDictionary<TKey, TValue> GetSnapshot()
        {
            var result = new Dictionary<TKey, TValue>(Capacity);
            foreach (var subCache in subCaches)
            {
                foreach (var key in subCache)
                {
                    result.Add(key, subCache[key]);
                }
            }
            return result;
        }
        
        #region IManagedCache<TKey, TValue> implementation
        public override bool Contains(TKey key)
        {
            return GetSubCache(key).Contains(key);
        }

        public override bool TryGet(TKey key, out TValue value)
        {
            return GetSubCache(key).TryGet(key, out value);
        }

        public override TValue Get(TKey key)
        {
            return GetSubCache(key).Get(key);
        }

        public override TValue GetOrAdd(TKey key, TValue value)
        {
            return GetSubCache(key).GetOrAdd(key, value);
        }

        public override TValue GetOrAdd<TContext>(TKey key, Func<TContext, TValue> updater, TContext context)
        {
            return GetSubCache(key).GetOrAdd(key, updater, context);
        }

        public override bool Add(TKey key, TValue value)
        {
            return GetSubCache(key).Add(key, value);
        }

        public override void Put(TKey key, TValue value)
        {
            GetSubCache(key).Put(key, value);
        }

        public override bool Remove(TKey key)
        {
            return GetSubCache(key).Remove(key);
        }

        public override int Capacity
        {
            get
            {
                return NumberOfWays * SubCacheCapacity;
            }
        }

        public override int Count
        {
            get
            {
                return subCaches.Sum(subCache => subCache.Count);
            }
        }
        #endregion
    }
}