#region Copyright

/*
 * Copyright (C) 2018 Larry Lopez
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */

#endregion Copyright

using System;
using System.Collections.Generic;

namespace Kettle.Searching
{
    /// <summary>
    /// Provides an abstract base class for implementing the <see cref="ISearch{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to search. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public abstract class SearchBase<T> : ISearch<T> where T : IComparable<T>
    {
        private const string _InvalidOffsetCountMessage = "Invalid offset and count.";
        protected IComparer<T> _comparer;

        /// <summary>
        /// Initializes a <see cref="SearchBase{T}"/> class.
        /// </summary>
        protected SearchBase() : this(Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a <see cref="SearchBase{T}"/> class.
        /// </summary>
        /// <param name="comparer">The custom comparer to use for searching.</param>
        protected SearchBase(IComparer<T> comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Gets and sets the default <see cref="IComparer{T}"/> to use for searching.
        /// </summary>
        /// <remarks>Defaults to <see cref="Comparer{T}.Default"/></remarks>
        public virtual IComparer<T> Comparer { get => _comparer; set => _comparer = value; }

        /// <summary>
        /// Performs a search on the entire <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        public virtual int Search(IList<T> list, T item)
        {
            return Search(list, item, 0, list.Count, _comparer);
        }

        /// <summary>
        /// Performs a search on the entire <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="comparer">The custom comparer to use.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        public virtual int Search(IList<T> list, T item, IComparer<T> comparer)
        {
            return Search(list, item, 0, list.Count, comparer);
        }

        /// <summary>
        /// Performs a search on a section of the <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        public virtual int Search(IList<T> list, T item, int start, int count)
        {
            return Search(list, item, 0, list.Count, _comparer);
        }

        /// <summary>
        /// Performs a search on a section of the <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <param name="comparer">The custom comparer to use.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        public abstract int Search(IList<T> list, T item, int start, int count, IComparer<T> comparer);

        /// <summary>
        /// A helper function for validating the parameters passed to the main search function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="start">The starting index of the first object to search.</param>
        /// <param name="count">The number of objects to include in the search.</param>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="IList{T}"/> passed in is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when start is less than zero or greater than the size of the list.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the start plus the count is greater than the size of the list.
        /// </exception>
        protected void SearchValidationCheck(IList<T> list, int start, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((uint)start >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (list.Count - start < count)
            {
                throw new ArgumentException(_InvalidOffsetCountMessage);
            }
        }
    }
}