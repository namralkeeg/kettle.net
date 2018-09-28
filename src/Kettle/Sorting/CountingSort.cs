using System;
using System.Collections.Generic;

namespace Kettle.Sorting
{
    /// <summary>
    /// An Counting Sort implementation of the <see cref="IKeyedSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort.</typeparam>
    /// <remarks>
    /// Counting Sort is a non-comparison type sort. It uses a function that returns an integer key instead.
    /// </remarks>
    public sealed class CountingSort<T> : KeyedSort<T>
    {
        #region Fields

        private const string _NonNegMessage = "Key values cannot be negative.";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="CountingSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="object.GetHashCode"/> for key generation.
        /// </remarks>
        public CountingSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="CountingSort{T}"/> class.
        /// </summary>
        /// <param name="keyFunction">
        /// A function <see cref="Func{T, TResult}"/> that returns an int key based on the value of T.
        /// </param>
        /// <remarks>Defaults to <see cref="object.GetHashCode"/> for key generation if null.</remarks>
        public CountingSort(Func<T, int> keyFunction) : base(keyFunction)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            int maxValue = GetMax(list, start, count);
            var countBucket = new int[maxValue + 1];
            int end = start + count;

            // Store the count for each key.
            for (int i = start; i < end; i++)
            {
                countBucket[_keyFunction(list[i])]++;
            }

            // Change countBucket[i] so that it contains the actual position of the item
            // in the output array.
            for (int i = 1; i <= maxValue; i++)
            {
                countBucket[i] += countBucket[i - 1];
            }

            // Array to build the sorted list.
            var sortedArray = new T[count];

            // Build the output array
            for (int i = start; i < end; i++)
            {
                sortedArray[countBucket[_keyFunction(list[i])] - 1] = list[i];
                --countBucket[_keyFunction(list[i])];
            }

            // Copy the sorted array back to the original.
            int sortedIndex = 0;
            for (int i = start; i < end; i++)
            {
                list[i] = sortedArray[sortedIndex++];
            }
        }

        /// <summary>
        /// Gets the maximum key for all elements in the list.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>The max int key for all elements in the list.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a key is less than zero.</exception>
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