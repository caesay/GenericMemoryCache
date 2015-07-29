using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class LfuEvictionPolicy<TKey, TValue> : CachePolicyBase<TKey, TValue>
    {
        public LfuEvictionPolicy()
            : base(EvictionPriority.PolicyImplied)
        {
        }

        #region ICachePolicy<TKey, TValue> implementation
        public override Type GetCacheEvictionType()
        {
            return typeof(LfuCacheEviction<TKey, TValue>);
        }
        #endregion
    }
}