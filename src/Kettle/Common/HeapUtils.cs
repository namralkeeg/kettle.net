using System;
using System.Collections.Generic;

namespace Kettle.Common
{
    public static class HeapUtils
    {
        /// <summary>
        /// Checks if all the elements in the array are a max heap.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <returns>True if the array is a max heap, false otherwise.</returns>
        /// <remarks>
        /// The objects to compare must implement the <see cref="IComparable{T}"/> interface.
        /// </remarks>
        public static bool IsHeap<T>(T[] array) where T : IComparable<T>
        {
            int end = array.Length - 1;
            return IsHeapUntil(array, 0, end, Comparer<T>.Default) == end;
        }

        /// <summary>
        /// Checks if the elements in range [start, end] are a max heap.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <returns>True if the array is a max heap, false otherwise.</returns>
        /// <remarks>
        /// The objects to compare must implement the <see cref="IComparable{T}"/> interface.
        /// </remarks>
        public static bool IsHeap<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            return IsHeapUntil(array, start, end, Comparer<T>.Default) == end;
        }

        /// <summary>
        /// Checks if the elements in range [start, end] are a max heap.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to use for comparing objects in the heap.
        /// </param>
        /// <returns>True if the array is a max heap, false otherwise.</returns>
        public static bool IsHeap<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            return IsHeapUntil(array, start, end, comparer) == end;
        }

        /// <summary>
        /// Examines the range [start, end] and finds the largest range beginning at start which is a
        /// max heap.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <returns>The upper bound of the largest range beginning at start which is a max heap.</returns>
        public static int IsHeapUntil<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            return IsHeapUntil(array, start, end, Comparer<T>.Default);
        }

        /// <summary>
        /// Examines the range [start, end] and finds the largest range beginning at start which is a
        /// max heap.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to use for comparing objects in the heap.
        /// </param>
        /// <returns>The upper bound of the largest range beginning at start which is a max heap.</returns>
        public static int IsHeapUntil<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            ValidateParams(array, start, end, comparer);

            int count = end - start + 1;
            int parent = start;
            for (int child = 1; child < count; child++)
            {
                if (comparer.Compare(array[parent], array[child]) < 0)
                {
                    return child;
                }

                if ((child & 1) == 0)
                {
                    parent++;
                }
            }

            return end;
        }

        /// <summary>
        /// Constructs a max heap from all elements of the array.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        public static void MakeHeap<T>(T[] array) where T : IComparable<T>
        {
            MakeHeap(array, 0, array.Length - 1, Comparer<T>.Default);
        }

        /// <summary>
        /// Constructs a max heap in the range [start, end].
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        public static void MakeHeap<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            MakeHeap(array, start, end, Comparer<T>.Default);
        }

        /// <summary>
        /// Constructs a max heap in the range [start, end].
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to use for comparing objects in the heap.
        /// </param>
        public static void MakeHeap<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            ValidateParams(array, start, end, comparer);

            int count = end - start + 1;
            if (count > 1)
            {
                for (int i = start + (int)Math.Floor((double)count / 2) - 1; i > start; i--)
                {
                    SiftDownMax(array, i, end, comparer);
                }
            }
        }

        /// <summary>
        /// Swaps the value in the position start and the value in the position end and makes the
        /// subrange [start, end-1] into a max heap. This has the effect of removing the start
        /// (largest) element from the heap defined by the range [start, end].
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <returns>The start element of the max heap.</returns>
        public static T PopHeap<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            return PopHeap(array, start, end, Comparer<T>.Default);
        }

        /// <summary>
        /// Swaps the value in the position start and the value in the position end and makes the
        /// subrange [start, end-1] into a max heap. This has the effect of removing the start
        /// (largest) element from the heap defined by the range [start, end].
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to use for comparing objects in the heap.
        /// </param>
        /// <returns>The start element of the max heap.</returns>
        public static T PopHeap<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            ValidateParams(array, start, end, comparer);

            int count = end - start + 1;
            T item = array[start];
            array[start] = array[end];
            array[end] = default(T);

            if (count > 1)
            {
                SiftDownMax(array, start, end - 1, comparer);
            }

            return item;
        }

        /// <summary>
        /// Inserts the element at the position end into the max heap defined by the range [start,
        /// end] and makes the subrange [start, end] into a max heap.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <param name="item">The object to push onto the max heap.</param>
        public static void PushHeap<T>(T[] array, int start, int end, T item) where T : IComparable<T>
        {
            PushHeap(array, start, end, Comparer<T>.Default, item);
        }

        /// <summary>
        /// Inserts the element at the position end into the max heap defined by the range [start,
        /// end] and makes the subrange [start, end] into a max heap.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to use for comparing objects in the heap.
        /// </param>
        /// <param name="item">The object to push onto the max heap.</param>
        public static void PushHeap<T>(T[] array, int start, int end, IComparer<T> comparer, T item)
        {
            ValidateParams(array, start, end, comparer);

            int count = end - start + 1;
            array[end] = item;
            if (count > 1)
            {
                SiftUpMax(array, start, end, comparer);
            }
        }

        /// <summary>
        /// Converts the max heap into a sorted range in ascending order. The resulting range no
        /// longer has the heap property.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        public static void SortHeap<T>(T[] array) where T : IComparable<T>
        {
            SortHeap(array, 0, array.Length - 1, Comparer<T>.Default);
        }

        /// <summary>
        /// Converts the max heap [start, end] into a sorted range in ascending order. The resulting
        /// range no longer has the heap property.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        public static void SortHeap<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            SortHeap(array, start, end, Comparer<T>.Default);
        }

        /// <summary>
        /// Converts the max heap [start, end] into a sorted range in ascending order. The resulting
        /// range no longer has the heap property.
        /// </summary>
        /// <typeparam name="T">The object type stored in the array.</typeparam>
        /// <param name="array">The array to check for a max heap.</param>
        /// <param name="start">The index of the first item in the array to check.</param>
        /// <param name="end">The index of the last item in the array to check.</param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to use for comparing objects in the heap.
        /// </param>
        public static void SortHeap<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            ValidateParams(array, start, end, comparer);

            if (start == end)
            {
                return;
            }

            int count = end - start + 1;
            if (count > 1)
            {
                while (end - start > 1)
                {
                    Utils.Swap(ref array[start], ref array[end]);
                    SiftDownMax(array, start, --end, comparer);
                }
            }
        }

        internal static void SiftDownMax<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            if (start == end)
            {
                return;
            }

            int left = Left(start);
            int right = Right(start);
            int largest = start;

            if ((left <= end) && (comparer.Compare(array[left], array[start]) > 0))
            {
                largest = left;
            }

            if ((right <= end) && (comparer.Compare(array[right], array[largest]) > 0))
            {
                largest = right;
            }

            if (largest != start)
            {
                Utils.Swap(ref array[start], ref array[largest]);
                SiftDownMax(array, largest, end, comparer);
            }
        }

        internal static void SiftUpMax<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            if (start == end)
            {
                return;
            }

            int count = end - start + 1;
            if (count > 1)
            {
                int index = end;
                int parent = Parent(index);

                while ((index != 0) && (comparer.Compare(array[parent], array[index]) < 0))
                {
                    Utils.Swap(ref array[parent], ref array[index]);
                    index = parent;
                    parent = Parent(index);
                }
            }
        }

        private static int Left(int i)
        {
            return (i << 1) + 1; // (i * 2) + 1
        }

        private static int Right(int i)
        {
            return (i << 1) + 2; // (i * 2) + 2
        }

        private static int Parent(int i)
        {
            return (i - 1) >> 1; // (i - 1) / 2
        }

        private static void ValidateParams<T>(T[] array, int start, int end, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length == 0)
            {
                throw new InvalidOperationException("Invalid operation on an empty heap.");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            if ((uint)start >= array.Length || (uint)start > end)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if ((uint)end >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
        }
    }
}