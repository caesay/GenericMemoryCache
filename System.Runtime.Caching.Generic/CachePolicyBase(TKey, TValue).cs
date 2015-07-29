using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class CachePolicyBase<TKey, TValue> : ICachePolicy<TKey, TValue>
    {
        protected CachePolicyBase(EvictionPriority evictionPriority)
        {
            EvictionPriority = evictionPriority;
        }

        #region ICachePolicy<TKey, TValue> implementation
        public virtual Type GetCacheEvictionType()
        {
            throw new NotImplementedException();
        }

        public CacheAccessCallback<TKey, TValue> OnGet { get; set; }

        public CacheAccessCallback<TKey, TValue> OnAdd { get; set; }

        public CacheAccessCallback<TKey, TValue> OnPut { get; set; }

        public CacheEvictionCallback<TKey, TValue> OnEvict { get; set; }

        public EvictionPriority EvictionPriority { get; private set; }
        #endregion
    }
}