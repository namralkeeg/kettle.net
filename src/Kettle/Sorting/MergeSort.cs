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

namespace Kettle.Sorting
{
    /// <summary>
    /// A Merge Sort implementation of the <see cref="ICompareSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public class MergeSort<T> : CompareSort<T> where T : IComparable<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="MergeSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/> which should
        /// be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public MergeSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="MergeSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public MergeSort(IComparer<T> comparer) : base(comparer)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            // Allocate a temp array only once at the beginning.
            var tempArray = new T[count];

            // Do the merge sort.
            MergeSortInternal(list, tempArray, start, start + count - 1);

            // Clear out the temp array and remove it.
            Array.Clear(tempArray, 0, count);
            tempArray = null;
        }

        /// <summary>
        /// Merges two subarrays from the original list into the temp array.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="tempArray">Temporary working array for merging and sorting.</param>
        /// <param name="left">Start index of the first subarray.</param>
        /// <param name="middle">End inclusive index of the first subarray.</param>
        /// <param name="right">End index of the second subarray.</param>
        private void Merge(IList<T> list, T[] tempArray, int left, int middle, int right)
        {
            // Create indexes to track sorting progress.
            int leftIndex, rightIndex, tempIndex;

            // Initial indexes of first and second subarrays
            leftIndex = left;
            rightIndex = middle + 1;

            // Starting index of the temp array.
            tempIndex = 0;

            // Sort and merge into the temp array.
            while ((leftIndex <= middle) && (rightIndex <= right))
            {
                if (Comparer.Compare(list[leftIndex], list[rightIndex]) <= 0)
                {
                    tempArray[tempIndex++] = list[leftIndex++];
                }
                else
                {
                    tempArray[tempIndex++] = list[rightIndex++];
                }
            }

            // Copy any remaining elements on the left.
            while (leftIndex <= middle)
            {
                tempArray[tempIndex++] = list[leftIndex++];
            }

            // Copy any remaining elements on the right.
            while (rightIndex <= right)
            {
                tempArray[tempIndex++] = list[rightIndex++];
            }

            // Copy the sorted temp array back to the original sub array.
            int end = right - left + 1;
            for (int i = 0; i < end; i++)
            {
                list[i + left] = tempArray[i];
            }
        }

        /// <summary>
        /// Internal merge sort recursive function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="tempArray">Temporary working array.</param>
        /// <param name="left">Starting inclusive index of the list to work with.</param>
        /// <param name="right">End inclusive index of the list to work with.</param>
        private void MergeSortInternal(IList<T> list, T[] tempArray, int left, int right)
        {
            if (left < right)
            {
                // Find the mid point.
                int middle = left + ((right - left) >> 1); // Same as (left + right) / 2

                // Sort the first half.
                MergeSortInternal(list, tempArray, left, middle);

                // Sort the second half.
                MergeSortInternal(list, tempArray, middle + 1, right);

                // Merge the sorted halves.
                Merge(list, tempArray, left, middle, right);
            }
        }

        #endregion Methods
    }
}