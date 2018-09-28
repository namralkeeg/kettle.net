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
    /// A LSD Radix Sort implementation of the <see cref="IKeyedSort{T}"/> interface.
    /// </summary>
    public sealed class RadixSort<T> : KeyedSort<T>
    {
        #region Fields

        private const int _DefaultNumericBase = 10;
        private const string _NonNegMessage = "Key values cannot be negative.";
        private const string _NumericBaseGreaterMessage = "Numeric base must be 2 or greater.";
        private int _numericBase;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="RadixSort{T}"/> class.
        /// </summary>
        /// <remarks>Defaults the numeric base to 10.</remarks>
        public RadixSort() : this(null, _DefaultNumericBase)
        {
        }

        /// <summary>
        /// Initializes a <see cref="RadixSort{T}"/> class.
        /// </summary>
        /// <param name="keyFunction">
        /// A function <see cref="Func{T, int}"/> that returns an int key based on the value of T.
        /// </param>
        public RadixSort(Func<T, int> keyFunction) : this(keyFunction, _DefaultNumericBase)
        {
        }

        /// <summary>
        /// Initializes a <see cref="RadixSort{T}"/> class.
        /// </summary>
        /// <param name="numericBase">The numeric base of the numbers to sort.</param>
        public RadixSort(int numericBase) : this(null, numericBase)
        {
        }

        /// <summary>
        /// Initializes a <see cref="RadixSort{T}"/> class.
        /// </summary>
        /// <param name="keyFunction">
        /// A function <see cref="Func{T, int}"/> that returns an int key based on the value of T.
        /// </param>
        /// <param name="numericBase">The numeric base of the numbers to sort.</param>
        public RadixSort(Func<T, int> keyFunction, int numericBase) : base(keyFunction)
        {
            NumericBase = numericBase;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The numeric base of the integers being sorted.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if tyring to set a value less than 2.</exception>
        public int NumericBase
        {
            get => _numericBase;
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), _NumericBaseGreaterMessage);
                }

                _numericBase = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            // Get the largest value and check for negative numbers.
            int maxValue = GetMax(list, start, count);
            int end = start + count;

            // Create the buckets for each digit.
            var buckets = new List<T>[NumericBase];
            int index = 0;

            // Iterate, sorting the list by each base - digit
            while ((int)Math.Pow(NumericBase, index) <= maxValue)
            {
                for (int i = start; i < end; i++)
                {
                    // Find the base digit from the number.
                    var key = _keyFunction(list[i]);
                    var digit = (key / (int)Math.Pow(NumericBase, index)) % NumericBase;

                    if (buckets[digit] == null)
                    {
                        buckets[digit] = new List<T>();
                    }

                    // Add the number to the correct bucket.
                    buckets[digit].Add(list[i]);
                }

                int startIndex = start;
                // Update the list with what's in the buckets.
                for (int i = 0; i < buckets.Length; i++)
                {
                    if (buckets[i] == null)
                    {
                        continue;
                    }

                    int bucketCount = buckets[i].Count;
                    for (int j = 0; j < bucketCount; j++)
                    {
                        list[startIndex++] = buckets[i][j];
                    }
                }

                // Clean out the buckets.
                for (int i = 0; i < buckets.Length; i++)
                {
                    if (buckets[i] != null)
                    {
                        buckets[i].Clear();
                        buckets[i] = null;
                    }
                }

                index++;
            }
        }

        /// <summary>
        /// Finds the largest number in the list.
        /// </summary>
        /// <param name="list">The <see cref="IList{int}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>The largest integer in the list.</returns>
        /// <exception cref="InvalidOperationException">Thrown if any integers are negative.</exception>
        private int GetMax(IList<T> list, int start, int count)
        {
            int maxValue = 0;
            int end = start + count;
            for (int i = start; i < end; i++)
            {
                var key = _keyFunction(list[i]);
                if (key < 0)
                {
                    throw new InvalidOperationException(_NonNegMessage);
                }

                if (key > maxValue)
                {
                    maxValue = key;
                }
            }

            return maxValue;
        }

        #endregion Methods
    }
}