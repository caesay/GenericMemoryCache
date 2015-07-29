using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the eviction priority used by an evicting cache and its policy's eviction strategy.
    /// </summary>
    public enum EvictionPriority
    {
        /// <summary>
        /// Both the priority of eviction and its order are implied by the cache policy's eviction strategy.
        /// </summary>
        PolicyImplied,

        /// <summary>
        /// The priority of eviction is *first in order* of the cache policy's eviction strategy (e.g., LRU).
        /// </summary>
        FirstInOrder,

        /// <summary>
        /// The priority of eviction is *last in order* of the cache policy's eviction strategy (e.g., MRU).
        /// </summary>
        LastInOrder
    }
}