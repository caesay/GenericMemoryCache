using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public class FifoEvictionPolicy<TKey, TValue> : CachePolicyBase<TKey, TValue>
    {
        protected FifoEvictionPolicy(EvictionPriority priority)
            : base(priority)
        {
            if (priority == EvictionPriority.PolicyImplied)
            {
                throw new ArgumentOutOfRangeException("priority", "cannot be implied");
            }
        }
    }
}