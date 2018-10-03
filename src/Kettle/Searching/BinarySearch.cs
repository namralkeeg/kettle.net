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
    /// A Binary Search implementation of the <see cref="ISearch{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to search. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class BinarySearch<T> : SearchBase<T> where T : IComparable<T>
    {
        /// <summary>
        /// Initializes a <see cref="BinarySearch{T}"/> class.
        /// </summary>
        public BinarySearch()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinarySearch{T}"/> class.
        /// </summary>
        /// <param name="comparer">The custom comparer to use for searching.</param>
        public BinarySearch(IComparer<T> comparer) : base(comparer)
        {
        }

        /// <inheritdoc/>
        public override int Search(IList<T> list, T item, int start, int count, IComparer<T> comparer)
        {
            SearchValidationCheck(list, start, count);

            if (comparer == null)
            {
                comparer = _comparer;
            }

            int right = start + count - 1;
            int left = start;
            int middle;

            while (left <= right)
            {
                middle = left + ((right - left) >> 1);    // same as (left + right) / 2
                switch (comparer.Compare(list[middle], item))
                {
                    case -1:
                        left = middle + 1;
                        break;

                    case 0:
                        return middle;

                    case 1:
                        right = middle - 1;
                        break;

                    default:
                        throw new InvalidOperationException("Invalid comparison return value. Must be 1, 0, or -1.");
                }
            }

            return -1;
        }
    }
}