using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class LruEvictionPolicy<TKey, TValue> : FifoEvictionPolicy<TKey, TValue>
    {
        public LruEvictionPolicy()
            : base(EvictionPriority.FirstInOrder)
        {
        }

        #region ICachePolicy<TKey, TValue> implementation
        public override Type GetCacheEvictionType()
        {
            return typeof(LruCacheEviction<TKey, TValue>);
        }
        #endregion
    }
}