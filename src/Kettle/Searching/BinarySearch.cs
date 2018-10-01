﻿#region Copyright

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
    public class BinarySearch<T> : ISearch<T> where T : IComparable<T>
    {
        /// <summary>
        /// Initializes a <see cref="BinarySearch{T}"/> class.
        /// </summary>
        public BinarySearch()
        {
        }

        /// <summary>
        /// Performs a binary search on the entire sorted <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        public int Search(IList<T> list, T item)
        {
            return Search(list, item, 0, list.Count, Comparer<T>.Default);
        }

        /// <summary>
        /// Performs a binary search on the entire sorted <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="comparer">The custom comparer to use.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        public int Search(IList<T> list, T item, IComparer<T> comparer)
        {
            return Search(list, item, 0, list.Count, comparer);
        }

        /// <summary>
        /// Performs a binary search on the entire sorted <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        /// <exception cref="ArgumentException">Thrown if start plus count is out of range.</exception>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if start is out of range.</exception>
        public int Search(IList<T> list, T item, int start, int count)
        {
            return Search(list, item, start, count, Comparer<T>.Default);
        }

        /// <summary>
        /// Performs a binary search on the entire sorted <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <param name="comparer">The custom comparer to use.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        /// <exception cref="ArgumentException">Thrown if start plus count is out of range.</exception>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if start is out of range.</exception>
        public int Search(IList<T> list, T item, int start, int count, IComparer<T> comparer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((uint)start > list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (list.Count - start < count)
            {
                throw new ArgumentException("Invalid offset and count.");
            }

            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
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