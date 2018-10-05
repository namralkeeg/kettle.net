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
using System.Linq;
using Kettle.Common;

namespace Kettle.Sorting
{
    /// <summary>
    /// A Shell Sort implementation of the <see cref="ICompareSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class ShellSort<T> : CompareSort<T> where T : IComparable<T>
    {
        #region Fields

        // Default gap sequence by Marcin Ciura https://oeis.org/A102549
        private static readonly int[] _DefaultGapSequence = { 1750, 701, 301, 132, 57, 23, 10, 4, 1 };

        private int[] _gapSequence;
        private bool _sorting = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/> which should
        /// be the <see cref="IComparable{T}"/> implementation for T. Defaults the Gap Sequence to
        /// the one by Marcin Ciura. https://oeis.org/A102549
        /// </remarks>
        public ShellSort() : this(_DefaultGapSequence, null)
        {
        }

        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public ShellSort(IComparer<T> comparer) : this(_DefaultGapSequence, comparer)
        {
        }

        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <param name="gapSequence">The gap sequence to use.</param>
        public ShellSort(IEnumerable<int> gapSequence) : this(gapSequence, null)
        {
        }

        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <param name="gapSequence">The gap sequence to use.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public ShellSort(IEnumerable<int> gapSequence, IComparer<T> comparer) : base(comparer)
        {
            _gapSequence = gapSequence.ToArray() ?? _DefaultGapSequence;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets and sets the gap sequence to use for the sorting algorithm.
        /// </summary>
        /// <remarks>If null it sets it to the default gap sequence.</remarks>
        public int[] GapSequence
        {
            get => Utils.CopyArray(_gapSequence);
            set
            {
                if (_sorting)
                {
                    throw new InvalidOperationException("Unable to change gap sequence while sorting.");
                }

                _gapSequence = Utils.CopyArray(value) ?? _DefaultGapSequence;
            }
        }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            _sorting = true;
            int end = start + count;

            foreach (int gap in _gapSequence)
            {
                int gapStart = gap + start;
                int j;
                for (int i = gapStart; i < end; i++)
                {
                    T temp = list[i];
                    for (j = i; j >= gap && _comparer.Compare(list[j - gapStart], temp) > 0; j -= gapStart)
                    {
                        list[j] = list[j - gap];
                    }

                    list[j] = temp;
                }
            }

            _sorting = false;
        }

        #endregion Methods
    }
}