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

namespace Kettle.Sorting
{
    /// <summary>
    /// An Insertion sort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class InsertionSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="InsertionSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/> which should
        /// be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public InsertionSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="InsertionSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public InsertionSort(IComparer<T> comparer) : base(comparer)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Insertion Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            int i, j;
            int end = start + count;

            for (i = start + 1; i < end; i++)
            {
                T item = list[i];
                for (j = i - 1; (j >= 0) && (Comparer.Compare(list[j], item) > 0); j--)
                {
                    list[j + 1] = list[j];
                }

                list[j + 1] = item;
            }
        }

        #endregion Methods
    }
}