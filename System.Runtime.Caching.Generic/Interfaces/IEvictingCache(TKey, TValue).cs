using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the interface of an evicting cache.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the cache</typeparam>
    /// <typeparam name="TValue">The type of values in the cache</typeparam>
    public interface IEvictingCache<TKey, TValue> : IManagedCache<TKey, TValue>
    {
        /// <summary>
        /// Returns an instance of a generically-derived cache policy type, compatible with this cache.
        /// </summary>
        /// <param name="genericPolicyType">The generic type of the cache policy to instantiate</param>
        /// <param name="args">The arguments to pass on to the constructor of the generically-derived cache policy type</param>
        /// <returns>An instance of a generically-derived cache policy type, compatible with this cache</returns>
        ICachePolicy<TKey, TValue> CreatePolicy(Type genericPolicyType, params object[] args);

        /// <summary>
        /// Sets the cache's policy with a compatible instance of a generically-derived cache policy type.
        /// </summary>
        /// <param name="genericPolicyType">The generic type of the cache policy to instantiate</param>
        /// <param name="args">The arguments to pass on to the constructor of the generically-derived cache policy type</param>
        void SetPolicy(Type genericPolicyType, params object[] args);

        /// <summary>
        /// Gets or sets the cache's policy.
        /// </summary>
        ICachePolicy<TKey, TValue> Policy { get; set; }
    }
}