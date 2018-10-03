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
    /// An Exponential Search implementation of the <see cref="ISearch{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to search. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <remarks>Requires the list to already be sorted.</remarks>
    public sealed class ExponentialSearch<T> : SearchBase<T> where T : IComparable<T>
    {
        private readonly ISearch<T> _binarySearch;

        /// <summary>
        /// Initializes a <see cref="ExponentialSearch{T}"/> class.
        /// </summary>
        public ExponentialSearch()
        {
            _binarySearch = new BinarySearch<T>(_comparer);
        }

        /// <summary>
        /// Initializes a <see cref="ExponentialSearch{T}"/> class.
        /// </summary>
        /// <param name="comparer">The custom comparer to use for searching.</param>
        public ExponentialSearch(IComparer<T> comparer) : base(comparer)
        {
            _binarySearch = new BinarySearch<T>(_comparer);
        }

        /// <inheritdoc/>
        public override int Search(IList<T> list, T item, int start, int count, IComparer<T> comparer)
        {
            SearchValidationCheck(list, start, count);

            if (comparer == null)
            {
                comparer = _comparer;
            }

            // Check if it's the first item in the list.
            if (comparer.Compare(list[start], item) == 0)
            {
                return start;
            }

            int bound = start + 1;
            int end = start + count;
            while ((bound < end) && (comparer.Compare(list[bound], item) <= 0))
            {
                bound *= 2;
            }

            int boundCount = Math.Min(bound + 1, end) - (bound / 2);

            return _binarySearch.Search(list, item, bound / 2, boundCount, comparer);
        }
    }
}