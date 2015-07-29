using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic.Collections
{
    /// <summary>
    /// Represents the interface of a collection tracking the use counts of its items, which are unique values.
    /// </summary>
    /// <typeparam name="TUnique">The type of unique values in the collection</typeparam>
    public interface IUseTrackingCollection<TUnique> : ICollection<TUnique>
    {
        /// <summary>
        /// Increments by one the use count of the given item, which must be present in the collection.
        /// </summary>
        /// <param name="item">The existing item whose use count is to be incremented</param>
        void Use(TUnique item);

        /// <summary>
        /// Returns an enumerable that can be used to enumerate the (up to maxCount) least frequently used items in the collection.
        /// </summary>
        /// <param name="maxCount">The maximum count of least frequently used items to enumerate</param>
        /// <returns>An enumerable to enumerate the (up to maxCount) least frequently used items</returns>
        IEnumerable<TUnique> GetLeastFrequentlyUsed(int maxCount);

        /// <summary>
        /// Gets the total use count of items (sum of the use counts of all items) in the collection. 
        /// </summary>
        long TotalUseCount { get; }
    }
}