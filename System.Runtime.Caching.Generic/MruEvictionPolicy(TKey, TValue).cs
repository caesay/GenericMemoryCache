using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class MruEvictionPolicy<TKey, TValue> : FifoEvictionPolicy<TKey, TValue>
    {
        public MruEvictionPolicy()
            : base(EvictionPriority.LastInOrder)
        {
        }

        #region ICachePolicy<TKey, TValue> implementation
        public override Type GetCacheEvictionType()
        {
            return typeof(MruCacheEviction<TKey, TValue>);
        }
        #endregion
    }
}