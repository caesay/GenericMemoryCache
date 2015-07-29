using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic.Collections
{
    /// <summary>
    /// Represents a doubly linked list of unique values, with O(1) random access (to n-th item).
    /// </summary>
    /// <typeparam name="TUnique">The type of unique values in the list</typeparam>
    public interface IDoubleEndedUniqueCollection<TUnique> : ICollection<TUnique>
    {
        /// <summary>
        /// Gets the "head" of the list.
        /// </summary>
        TUnique First { get; }

        /// <summary>
        /// Gets the "tail" of the list.
        /// </summary>
        TUnique Last { get; }
    }
}