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
using Kettle.Common;

namespace Kettle.Sorting
{
    /// <summary>
    /// A Bubble Sort implementation of the <see cref="ICompareSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort.</typeparam>
    /// <remarks>Includes an optimization where by after every pass, all elements after the 
    /// last swap are sorted, and do not need to be checked again</remarks>
    public sealed class BubbleSort<T> : CompareSort<T> where T : IComparable<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="BubbleSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public BubbleSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BubbleSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public BubbleSort(IComparer<T> comparer) : base(comparer)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Bubble Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            int itemsLeft = count;
            int lastSwapped;
            int end = start + count;
            do
            {
                lastSwapped = 0;
                for (int i = start + 1; i < end; i++)
                {
                    if (Comparer.Compare(list[i - 1], list[i]) > 0)
                    {
                        Utils.Swap(list, i - 1, i);
                        lastSwapped = i;
                    }
                }

                itemsLeft = lastSwapped;
            } while (itemsLeft > 0);
        }

        #endregion Methods
    }
}