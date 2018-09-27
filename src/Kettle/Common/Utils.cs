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

namespace Kettle.Common
{
    internal static class Utils
    {
        #region Array Functions

        /// <summary>
        /// Generic function to create a shallow copy of a jagged array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source jagged array to copy from.</param>
        /// <returns>A shallow copy of the jagged array provided.</returns>
        internal static T[][] CopyArray<T>(T[][] source)
        {
            int length = source?.Length ?? 0;
            T[][] destination = new T[length][];

            for (int i = 0; i < length; ++i)
            {
                T[] innerArray = source[i];
                int innerLength = innerArray.Length;
                T[] newArray = new T[innerLength];
                Array.Copy(innerArray, newArray, innerLength);
                destination[i] = newArray;
            }

            return destination;
        }

        /// <summary>
        /// Generic function to create a shallow copy of an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">>The source array to copy from.</param>
        /// <returns>A shallow copy of the array provided.</returns>
        internal static T[] CopyArray<T>(T[] source)
        {
            int dataLength = source?.Length ?? 0;
            T[] temp = new T[dataLength];
            Array.Copy(source, 0, temp, 0, dataLength);

            return temp;
        }

        internal static byte[] CopyArray(byte[] source)
        {
            byte[] temp = new byte[source.Length] ?? EmptyArray<byte>.Value;
            Buffer.BlockCopy(source, 0, temp, 0, temp.Length);

            return temp;
        }

        internal static void Fill<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = value;
            }
        }

        internal static void Fill<T>(IList<T> array, T value)
        {
            for (int i = 0; i < array.Count; ++i)
            {
                array[i] = value;
            }
        }

        internal static class EmptyArray<T>
        {
            #region Fields

            internal static readonly T[] Value = new T[0];

            #endregion Fields
        }

        #endregion Array Functions

        #region Swap Functions

        /// <summary>
        /// Swaps the left and right values.
        /// </summary>
        /// <typeparam name="T">The element type of the items to swap.</typeparam>
        /// <param name="left">Value on the left to swap.</param>
        /// <param name="right">Value on the right to swap.</param>
        internal static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }

        /// <summary>
        /// Swaps two values in the generic array at index left and index right.
        /// </summary>
        /// <typeparam name="T">The element type of the <see cref="Array"/>.</typeparam>
        /// <param name="array"><see cref="Array"/> of generic T items. </param>
        /// <param name="left">Index of the first item to swap in the array.</param>
        /// <param name="right">Index of the second item to swap in the array.</param>
        internal static void Swap<T>(T[] array, int left, int right)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (left == right)
            {
                return;
            }

            T temp = array[left];
            array[left] = array[right];
            array[right] = temp;
        }

        /// <summary>
        /// Swaps two values in the <see cref="IList{T}"/> at index left and index right.
        /// </summary>
        /// <typeparam name="T">The element type of the <see cref="IList{T}"/></typeparam>
        /// <param name="list"><see cref="IList{T}"/> of generic T items.</param>
        /// <param name="left">Index of the first item to swap in the <see cref="IList{T}"/>.</param>
        /// <param name="right">Index of the second item to swap in the <see cref="IList{T}"/>.</param>
        internal static void Swap<T>(IList<T> list, int left, int right)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (left == right)
            {
                return;
            }

            T temp = list[left];
            list[left] = list[right];
            list[right] = temp;
        }

        #endregion Swap Functions
    }
}