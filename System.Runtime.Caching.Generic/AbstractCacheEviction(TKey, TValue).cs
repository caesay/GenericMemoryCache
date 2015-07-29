using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public abstract class AbstractCacheEviction<TKey, TValue> : AbstractCacheState<TKey, TValue>, ICacheEviction<TKey, TValue>
    {
        protected AbstractCacheEviction(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(owner, capacity)
        {
            Evictables = CreateEvictableCollection();
        }

        protected override bool TracksChanges()
        {
            return Evictables != null;
        }

        protected override void Forget(TKey key)
        {
            base.Forget(key);
            if (TracksChanges())
            {
                Evictables.Remove(key);
            }
        }

        protected virtual ICollection<TKey> CreateEvictableCollection()
        {
            var type = GetEvictableCollectionType();
            return type != null ? (ICollection<TKey>)Activator.CreateInstance(type, Capacity > 0 ? new object[] { Capacity } : null) : null;
        }

        protected abstract Type GetEvictableCollectionType();

        protected abstract int GetEvictionSize();

        protected abstract bool GetNextEvictedKey(out TKey key);

        protected ICollection<TKey> Evictables { get; private set; }

        #region ICacheEviction<TKey, TValue> implementation
        public bool Evict(EvictionReason reason, TKey key)
        {
            var onEvict = Policy != null ? Policy.OnEvict : null;
            TValue value;
            if (TryGetValue(key, out value))
            {
                Forget(key);
                if (onEvict != null)
                {
                    onEvict(Source, key, value, reason);
                }
                return true;
            }
            return false;
        }

        public bool Evict()
        {
            if (TracksChanges())
            {
                TKey key;
                var toEvict = GetEvictionSize();
                var evicted = 0;
                // Try to evict as many items as we're supposed to...
                while (evicted < toEvict && GetNextEvictedKey(out key))
                {
                    if (!Evict(EvictionReason.Policy, key))
                    {
                        // Should never happen if the implementation is correct, but just in case...
                        throw new InvalidOperationException("cache eviction out-of-sync with data");
                    }
                    evicted++;
                }
                // It's a success iff we have effectively evicted
                // exactly as many items as we had decided to...
                return evicted == toEvict;
            }
            // ... or if we're not supposed to evict anything anyway
            return true;
        }
        #endregion
    }
}