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
    public abstract class SortAlgorithm<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        #region Fields

        /// <summary>
        /// The <see cref="IComparer{T}"/> to use for comparing objects while sorting.
        /// </summary>
        protected IComparer<T> _comparer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="SortAlgorithm{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/> which should
        /// be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        protected SortAlgorithm()
        {
            _comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a <see cref="SortAlgorithm{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        protected SortAlgorithm(IComparer<T> comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="IComparer{T}"/> to use for sorting the objects.
        /// </summary>
        /// <remarks>Defaults to <see cref="Comparer{T}.Default"/></remarks>.
        public virtual IComparer<T> Comparer
        {
            get => _comparer;
            set => _comparer = value ?? Comparer<T>.Default;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the <see cref="Comparer"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        public void Sort(IList<T> list)
        {
            Sort(list, 0, list.Count);
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the <see cref="Comparer"/> when
        /// implemented in a derived class.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        public abstract void Sort(IList<T> list, int start, int count);

        /// <summary>
        /// A helper function for validating the parameters passed to the main sort function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="IList{T}"/> passed in is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when start is less than zero or greater than the size of the list.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the start plus the count is greater than the size of the list.
        /// </exception>
        protected void SortValidationCheck(IList<T> list, int start, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((start < 0) || (start > list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (list.Count - start < count)
            {
                throw new ArgumentException($"{nameof(start)} plus {nameof(count)} is out of list range.");
            }
        }

        #endregion Methods
    }
}