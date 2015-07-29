using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public abstract class AbstractCacheState<TKey, TValue> : Dictionary<TKey, TValue>, ICacheState<TKey, TValue>
    {
        protected AbstractCacheState(IEvictingCache<TKey, TValue> owner, int capacity)
            : base(capacity)
        {
            if ((Capacity = capacity) < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "must be greater than or equal zero");
            }
            if (owner == null)
            {
                throw new ArgumentNullException("owner", "cannot be null");
            }
            Source = owner.Parent;
            Policy = owner.Policy;
        }

        protected abstract bool TracksChanges();

        protected abstract void Before(CacheAccess access, TKey key, TValue value);

        protected abstract void After(CacheAccess access, TKey key, TValue value);

        protected virtual void Forget(TKey key)
        {
            Remove(key);
        }

        protected IManagedCache<TKey, TValue> Source { get; private set; }

        protected ICachePolicy<TKey, TValue> Policy { get; private set; }

        protected int Capacity { get; private set; }

        #region ICacheState<TKey, TValue> implementation
        public bool Handle(CacheAccess access, TKey key, ref TValue value, bool isGetOrPut)
        {
            bool found = ContainsKey(key);
            if (!found || isGetOrPut)
            {
                if (!found && access == CacheAccess.Get)
                {
                    // Failure; can't Get (key not found)
                    return false;
                }
                if (TracksChanges())
                {
                    Before(access, key, value);
                }
                try
                {
                    var onAccess =
                        Policy != null ?
                        access == CacheAccess.Get ? Policy.OnGet :
                        access == CacheAccess.Set ? isGetOrPut ? Policy.OnPut : Policy.OnAdd : null : null;
                    if (access == CacheAccess.Get)
                    {
                        // It's a Get
                        value = this[key];
                    }
                    else
                    {
                        if (!isGetOrPut)
                        {
                            // It's an Add
                            Add(key, value);
                        }
                        else
                        {
                            // It's a Put
                            this[key] = value;
                        }
                    }
                    switch (access)
                    {
                        case CacheAccess.Get:
                        // It's a cache hit, so give the policy a chance
                        // to mutate the data if so desired (via onAccess)
                        // e.g., for the sake of reordering the evictables
                        // as a side-effect of the cache hit
                        // (cf. next comment in "finally{...}" clause below)
                        case CacheAccess.Set:
                            if (onAccess != null)
                            {
                                onAccess(Source, key, value);
                            }
                            break;
                        default:
                            throw new NotSupportedException(access.ToString());
                    }
                }
                finally
                {
                    if (TracksChanges())
                    {
                        // Keep our own internal state consistent
                        // even if the policy happens to fail;
                        // the following is to keep the evictable
                        // key/value pairs in order;
                        // note that in the special case of LRU/MRU
                        // evictions, it's the passing of time
                        // that defines the order
                        // (see FifoCacheEviction<TKey, TValue>)
                        After(access, key, value);
                    }
                }
                // Success; could Get, Add, or Put
                return true;
            }
            // Failure; can't Add (key already in use)
            return false;
        }
        #endregion
    }
}