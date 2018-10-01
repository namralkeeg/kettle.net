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
    /// A Heap Sort implementation of the <see cref="ICompareSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class HeapSort<T> : CompareSort<T> where T : IComparable<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="HeapSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/> which should
        /// be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public HeapSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="HeapSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public HeapSort(IComparer<T> comparer) : base(comparer)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            // Build a max heap.
            for (int i = ((start + count) >> 1) - 1; i >= start; i--)
            {
                Heapify(list, i, count);
            }

            // Extract an element from the heap one at a time.
            for (int j = start + count - 1; j >= start; j--)
            {
                // Move the current root to the end.
                Utils.Swap(list, j, start);

                // Call max heapify on a reduced heap.
                Heapify(list, start, j);
            }
        }

        private void Heapify(IList<T> list, int index, int count)
        {
            int left = (index << 1) + 1;    // index * 2 + 1
            int right = (index << 1) + 2;   // index * 2 + 2
            int largest = index;            // Initialize largest as root

            // If left child is larger than root
            if ((left < count) && (Comparer.Compare(list[left], list[index]) > 0))
            {
                largest = left;
            }

            // If right child is larger than largest so far
            if ((right < count) && (Comparer.Compare(list[right], list[largest]) > 0))
            {
                largest = right;
            }

            // If largest is not root
            if (largest != index)
            {
                Utils.Swap(list, index, largest);

                // Recursively heapify the affected sub-tree
                Heapify(list, largest, count);
            }
        }

        #endregion Methods
    }
}