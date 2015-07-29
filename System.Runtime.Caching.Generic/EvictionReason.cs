using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the eviction reason used by an evicting cache and its policy's eviction strategy.
    /// </summary>
    public enum EvictionReason
    {
        /// <summary>
        /// The item was evicted through the algorithm implemented by the cache policy's eviction strategy.
        /// </summary>
        Policy,

        /// <summary>
        /// The item was evicted through a client call to the cache's Remove method.
        /// </summary>
        Removal
    }
}