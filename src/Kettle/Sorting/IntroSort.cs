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
    ///  An Introsort implementation of the <see cref="ICompareSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class IntroSort<T> : CompareSort<T> where T : IComparable<T>
    {
        #region Fields

        private const int _Threshold = 16;
        private readonly ICompareSort<T> _heapSort;
        private readonly ICompareSort<T> _insertionSort;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="IntroSort{T}"/> class.
        /// </summary>
        public IntroSort()
        {
            _insertionSort = new InsertionSort<T>(_comparer);
            _heapSort = new HeapSort<T>(_comparer);
        }

        /// <summary>
        /// Initializes a <see cref="IntroSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public IntroSort(IComparer<T> comparer) : base(comparer)
        {
            _insertionSort = new InsertionSort<T>(_comparer);
            _heapSort = new HeapSort<T>(_comparer);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="IComparer{T}"/> to use for sorting the objects.
        /// </summary>
        /// <remarks>Defaults to <see cref="Comparer{T}.Default"/></remarks>.
        public override IComparer<T> Comparer
        {
            get => _comparer;
            set
            {
                _comparer = value ?? Comparer<T>.Default;
                _insertionSort.Comparer = _comparer;
                _heapSort.Comparer = _comparer;
            }
        }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            int depthLimit = 2 * (int)Math.Floor(Math.Log(count, 2));
            int end = start + count - 1;
            IntroSortInternal(list, start, end, depthLimit);
        }

        /// <summary>
        /// Internal recursive function that performs the core Introsort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        /// <param name="depthLimit">Tracks the recursion cutoff depth.</param>
        private void IntroSortInternal(IList<T> list, int low, int high, int depthLimit)
        {
            int count = high - low + 1;

            // If the number of items is at the threshold, then use insertion sort.
            if (count <= _Threshold)
            {
                // Only one item in the list. Nothing to do here.
                if (count == 1)
                {
                    return;
                }

                // Only two items in the list.
                if (count == 2)
                {
                    if (_comparer.Compare(list[low], list[high]) > 0)
                    {
                        Utils.Swap(list, low, high);
                        return;
                    }
                }

                // Only 3 items in the list.
                if (count == 3)
                {
                    // Same as (low + high) / 2, but prevents an integer overflow.
                    int middle = low + ((high - low) >> 1);

                    if (_comparer.Compare(list[middle], list[low]) < 0)
                    {
                        Utils.Swap(list, low, middle);
                    }

                    if (_comparer.Compare(list[high], list[low]) < 0)
                    {
                        Utils.Swap(list, low, high);
                    }

                    if (_comparer.Compare(list[high], list[middle]) < 0)
                    {
                        Utils.Swap(list, middle, high);
                    }

                    return;
                }

                _insertionSort.Sort(list, low, count);
                return;
            }

            // If the recursion depth limit is reached do a heap sort instead.
            if (depthLimit == 0)
            {
                _heapSort.Sort(list, low, count);
                return;
            }

            depthLimit--;
            int partitionIndex = Partition(list, low, high);
            IntroSortInternal(list, low, partitionIndex - 1, depthLimit);
            IntroSortInternal(list, partitionIndex + 1, high, depthLimit);
        }

        /// <summary>
        /// Internal partition function to reorder the array so that all elements with values less
        /// than the pivot come before the pivot, while all elements with values greater than the
        /// pivot come after it (equal values can go either way).
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        /// <returns>The index of the pivot element.</returns>
        private int Partition(IList<T> list, int low, int high)
        {
            // Begin median-of-three pivot algorithm.
            // https://en.wikipedia.org/wiki/Quicksort#Choice_of_pivot

            // Same as (low + high) / 2, but prevents an integer overflow.
            int middle = low + ((high - low) >> 1);

            if (_comparer.Compare(list[middle], list[low]) < 0)
            {
                Utils.Swap(list, low, middle);
            }

            if (_comparer.Compare(list[high], list[low]) < 0)
            {
                Utils.Swap(list, low, high);
            }

            if (_comparer.Compare(list[high], list[middle]) < 0)
            {
                Utils.Swap(list, middle, high);
            }

            // Choose the middle as the pivot value.
            T pivotValue = list[middle];

            // Place the pivot at end - 1.
            Utils.Swap(list, middle, high - 1);

            // Low, High and the pivot are already partitioned relative to the pivot in median-of-three.
            int left = low;
            int right = high - 1;

            while (true)
            {
                while (_comparer.Compare(list[++left], pivotValue) < 0) ;

                while (_comparer.Compare(list[--right], pivotValue) > 0) ;

                if (left >= right)
                {
                    // Return the pivot back to the middle.
                    // Everything is now less than the pivot on the left and greater than on the right.
                    Utils.Swap(list, left, high - 1);
                    return left;
                }

                Utils.Swap(list, left, right);
            }
        }

        #endregion Methods
    }
}