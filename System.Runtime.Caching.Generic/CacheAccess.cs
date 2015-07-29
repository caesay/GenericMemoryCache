using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the cache access operation during notifications of a cache calling back into its client.
    /// </summary>
    public enum CacheAccess
    {
        /// <summary>
        /// Current operation is a read (cf. cache policy's OnGet).
        /// </summary>
        Get,

        /// <summary>
        /// Current operation is either an add or update (cf. cache policy's OnAdd or OnPut).
        /// </summary>
        Set
    }
}