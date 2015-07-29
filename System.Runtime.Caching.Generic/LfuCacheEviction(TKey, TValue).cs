using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    using Collections;

    public class LfuCacheEviction<TKey, TValue> : CacheEvictionBase<TKey, TValue>
    {
        private LinkedList<TKey> evicted = new LinkedList<TKey>();

        private LinkedListNode<TKey> evictable;

        public LfuCacheEviction(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(owner, capacity)
        {
        }

        protected virtual int GetLeastFrequentlyUsedScope()
        {
            return 1;
        }

        protected IUseTrackingCollection<TKey> UsedKeys
        {
            get
            {
                return (IUseTrackingCollection<TKey>)Evictables;
            }
        }

        protected override Type GetEvictableCollectionType()
        {
            return typeof(LfuEvictableCollection<TKey>);
        }

        protected override int GetEvictionSize()
        {
            var count = GetLeastFrequentlyUsedScope();
            evicted.Clear();
            foreach (var key in UsedKeys.GetLeastFrequentlyUsed(count))
            {
                evicted.AddLast(key);
            }
            evictable = evicted.First;
            return evicted.Count;
        }

        protected override bool GetNextEvictedKey(out TKey key)
        {
            if (evictable != null)
            {
                key = evictable.Value;
                evictable = evictable.Next;
                return true;
            }
            key = default(TKey);
            return false;
        }

        protected override void Before(CacheAccess access, TKey key, TValue value)
        {
            if (access == CacheAccess.Get)
            {
                UsedKeys.Use(key);
            }
        }

        protected override void After(CacheAccess access, TKey key, TValue value)
        {
            if (access == CacheAccess.Set)
            {
                Evictables.Add(key);
            }
        }
    }
}