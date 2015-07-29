using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public sealed class NoEvictionPolicy<TKey, TValue> : CachePolicyBase<TKey, TValue>
    {
        public NoEvictionPolicy()
            : base(EvictionPriority.PolicyImplied)
        {
        }

        #region ICachePolicy<TKey, TValue> implementation
        public override Type GetCacheEvictionType()
        {
            return typeof(NoCacheEviction<TKey, TValue>);
        }
        #endregion
    }
}