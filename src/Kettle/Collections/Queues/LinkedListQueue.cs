using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Kettle.Collections.Queues
{
    /// <summary>
    /// An Linked List Queue implemenatation of the <see cref="IQueue{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to store in the binary search tree.</typeparam>
    [Serializable]
    public class LinkedListQueue<T> : IQueue<T>, ISerializable, IDeserializationCallback
    {
        #region Fields

        private const string _ArrayNonZeroLBMessage = "Array has a non-zero lower bound.";
        private const string _DestArraySizeTooSmallMessage = "Destination array size is too small.";
        private const string _InvalidArrayTypeMessage = "Invalid array type.";
        private const string _OnlySingleDimArraysMessage = "Only single dimensional arrays are supported.";
        private const string _StartIndexZeroGreaterMessage = "Starting index must be 0 or greater.";

        private const string _CountName = "Count";
        private const string _ValuesName = "Data";
        private const string _VersionName = "Version";

        private int _count = 0;
        private QueueNode<T> _first = null;
        private QueueNode<T> _last = null;
        private long _version;
        private object _syncRoot;

        //A temporary variable which we need during deserialization.
        private SerializationInfo _siInfo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="LinkedListQueue{T}"/> class.
        /// </summary>
        public LinkedListQueue()
        {
        }

        /// <summary>
        /// Initializes a <see cref="LinkedListQueue{T}"/> class.
        /// </summary>
        /// <param name="collection">An <see cref="IEnumerable{T}"/> collection of objects to initialize the queue with.</param>
        public LinkedListQueue(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                Enqueue(item);
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected LinkedListQueue(SerializationInfo info, StreamingContext context)
        {
            _siInfo = info;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// A read-only property that gets the number of items in the queue.
        /// </summary>
        public int Count => _count; 

        /// <summary>
        /// A read-only property that tells if this collection is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// A read-only property that tells if the collection is synchronized.
        /// </summary>
        public bool IsSynchronized => false;

        /// <summary>
        /// Gets the synchronization root for this object.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds an item to the end of the <see cref="LinkedListQueue{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the queue.</param>
        public void Add(T item)
        {
            Enqueue(item);
        }

        /// <summary>
        /// Removes all items from the queue and resets the count.
        /// </summary>
        public void Clear()
        {
            QueueNode<T> current = _first;
            while (current != null)
            {
                QueueNode<T> temp = current;
                current = current.Next;
                temp.Invalidate();
            }

            _first = _last = null;
            _count = 0;
            _version++;
        }

        /// <summary>
        /// Checks if an item is in the queue.
        /// </summary>
        /// <param name="item">The object to check for in the queue.</param>
        /// <returns>True if the item is found, false otherwise.</returns>
        public bool Contains(T item)
        {
            // Use the default equality comparer for the type T.
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            QueueNode<T> node = _first;

            // If there are items in the queue.
            if (node != null)
            {
                if (item != null)
                {
                    do
                    {
                        // Use the equality comparer if the item isn't null.
                        if (comparer.Equals(node.Value, item))
                        {
                            return true;
                        }
                        node = node.Next;
                    } while (node != null);
                }
                else
                {
                    do
                    {
                        if (node.Value == null)
                        {
                            return true;
                        }
                        node = node.Next;
                    } while (node != null);
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the <see cref="LinkedListQueue{T}"/> elements to an existing one-dimensional <see cref="Array"/>.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from <see cref="LinkedListQueue{T}"/>. The Array must have zero-based indexing.
        /// </param>
        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        /// <summary>
        /// Copies the <see cref="LinkedListQueue{T}"/> elements to an existing one-dimensional <see cref="Array"/>,
        /// starting at the specified array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from <see cref="LinkedListQueue{T}"/>. The Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_count == 0)
            {
                return;
            }

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if ((uint)arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), _StartIndexZeroGreaterMessage);
            }

            if (array.Length - arrayIndex < _count)
            {
                throw new ArgumentOutOfRangeException(nameof(array), _DestArraySizeTooSmallMessage);
            }

            QueueNode<T> node = _first;
            if (node != null)
            {
                do
                {
                    array[arrayIndex++] = node.Value;
                    node = node.Next;
                } while (node != null);
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="LinkedListQueue{T}"/> to an <see cref="Array"/>,
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from <see cref="LinkedListQueue{T}"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        void ICollection.CopyTo(Array array, int index)
        {
            if (_count == 0)
            {
                return;
            }

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if ((uint)index >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), _StartIndexZeroGreaterMessage);
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException(_OnlySingleDimArraysMessage, nameof(array));
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException(_ArrayNonZeroLBMessage, nameof(array));
            }

            if (array.Length - index < _count)
            {
                throw new ArgumentOutOfRangeException(nameof(array), _DestArraySizeTooSmallMessage);
            }

            if (array is T[] testArray)
            {
                CopyTo(testArray, index);
            }
            else
            {
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                {
                    throw new ArgumentException(_InvalidArrayTypeMessage, nameof(array));
                }

                if (!(array is object[] objects))
                {
                    throw new ArgumentException(_InvalidArrayTypeMessage, nameof(array));
                }

                QueueNode<T> node = _first;
                try
                {
                    if (node != null)
                    {
                        do
                        {
                            objects[index++] = node.Value;
                            node = node.Next;
                        } while (node != null);
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException(_InvalidArrayTypeMessage, nameof(array));
                }
            }
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="LinkedListQueue{T}"/>.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="LinkedListQueue{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="LinkedListQueue{T}"/> is empty.
        /// </exception>
        public T Dequeue()
        {
            if (_count == 0 || _first == null)
            {
                throw new InvalidOperationException("Trying to dequeue from an empty queue.");
            }

            T removed = _first.Value;
            // This is the last item in the queue.
            if (_first == _last)
            {
                _first.Invalidate();
                _first = _last = null;
            }
            else
            {
                QueueNode<T> temp = _first;
                _first = _first.Next;
                _first.Previous = null;
                temp.Invalidate();
                temp = null;
            }

            _count--;
            _version++;
            return removed;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="LinkedListQueue{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="LinkedListQueue{T}"/>. The value can be null for reference types.
        /// </param>
        public void Enqueue(T item)
        {
            QueueNode<T> node = new QueueNode<T>(item);

            if (_last == null)
            {
                _first = _last = node;
                _count++;
                _version++;
                return;
            }

            _last.Next = node;
            node.Previous = _last;
            _last = node;

            _count++;
            _version++;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(_VersionName, _version);
            info.AddValue(_CountName, _count);

            if (_count != 0)
            {
                T[] array = new T[_count];
                CopyTo(array, 0);
                info.AddValue(_ValuesName, array, typeof(T[]));
            }
        }

        /// <inheritdoc/>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void OnDeserialization(object sender)
        {
            if (_siInfo == null)
            {
                return;
            }

            int realVersion = _siInfo.GetInt32(_VersionName);
            int count = _siInfo.GetInt32(_CountName);

            if (count > 0)
            {
                T[] array = (T[])_siInfo.GetValue(_ValuesName, typeof(T[]));
                if (array == null)
                {
                    throw new SerializationException("Missing values for serialization.");
                }

                foreach (T item in array)
                {
                    Enqueue(item);
                }
            }
            else
            {
                _first = _last = null;
            }

            _version = realVersion;
            _siInfo = null;
        }

        /// <summary>
        /// Returns the object at the beginning of the <see cref="LinkedListQueue{T}"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="LinkedListQueue{T}"/>.</returns>
        public T Peek()
        {
            if (_count == 0 || _first == null)
            {
                throw new InvalidOperationException("Trying to peek on an empty queue.");
            }

            if (_first != null)
            {
                return _first.Value;
            }

            return default(T);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the <see cref="LinkedListQueue{T}"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="LinkedListQueue{T}"/>.</returns>
        public T[] ToArray()
        {
            T[] buffer = new T[_count];
            CopyTo(buffer, 0);

            return buffer;
        }

        /// <summary>
        /// Copies the <see cref="LinkedListQueue{T}"/> elements to a new <see cref="List{T}"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="List{T}"/> contanint elements copied from the <see cref="LinkedListQueue{T}"/>.
        /// </returns>
        public List<T> ToList()
        {
            List<T> list = new List<T>(_count);
            list.AddRange(this);

            return list;
        }

        /// <summary>
        /// Tries to remove and return the object at the beginning of the <see cref="LinkedListQueue{T}"/>.
        /// </summary>
        /// <param name="result">The object that is removed from the beginning of the <see cref="LinkedListQueue{T}"/>.</param>
        /// <returns>True if the object was removed from the queue, false otherwise.</returns>
        public bool TryDequeue(out T result)
        {
            if (_first == null || _count == 0)
            {
                result = default(T);
                return false;
            }
            else
            {
                result = _first.Value;
                // This is the last item in the queue.
                if (_first == _last)
                {
                    _first.Invalidate();
                    _first = _last = null;
                }
                else
                {
                    QueueNode<T> temp = _first;
                    _first = _first.Next;
                    _first.Previous = null;
                    temp.Invalidate();
                    temp = null;
                }

                _count--;
                _version++;
                return true;
            }
        }

        /// <summary>
        /// Tries to return the object at the beginning of the <see cref="LinkedListQueue{T}"/> without removing it.
        /// </summary>
        /// <param name="result">The object at the beginning of the <see cref="LinkedListQueue{T}"/>.</param>
        /// <returns>True if there is at least one object in the queue, false otherwise.</returns>
        public bool TryPeek(out T result)
        {
            if ((_first != null) && (_count > 0))
            {
                result = _first.Value;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }

        #endregion Methods

        #region Structs

        internal struct Enumerator : IEnumerator<T>, IEnumerator
        {
            #region Fields

            private readonly long _version;
            private int _index;
            private QueueNode<T> _node;
            private LinkedListQueue<T> _queue;

            #endregion Fields

            #region Constructors

            internal Enumerator(LinkedListQueue<T> queue) : this()
            {
                _queue = queue;
                _version = _queue._version;
                _node = _queue._first;
                Current = default(T);
                _index = 0;
            }

            #endregion Constructors

            #region Properties

            public T Current { get; private set; }

            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || (_index == _queue.Count + 1))
                    {
                        throw new InvalidOperationException();
                    }

                    return Current;
                }
            }

            #endregion Properties

            #region Methods

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_version != _queue._version)
                {
                    throw new InvalidOperationException();
                }

                if (_node == null)
                {
                    _index = _queue.Count + 1;
                    return false;
                }

                _index++;
                Current = _node.Value;
                _node = _node.Next;

                return true;
            }

            public void Reset()
            {
                Current = default(T);
                _node = _queue._first;
                _index = 0;
            }

            #endregion Methods
        }

        #endregion Structs
    }

    internal sealed class QueueNode<T>
    {
        internal QueueNode()
        {
        }

        internal QueueNode(T value, QueueNode<T> next, QueueNode<T> previous) : this(value)
        {
            Next = next;
            Previous = previous;
        }

        internal QueueNode(T value)
        {
            Value = value;
        }

        internal QueueNode<T> Next { get; set; }
        internal QueueNode<T> Previous { get; set; }
        internal T Value { get; set; }

        internal void Invalidate()
        {
            Next = null;
            Previous = null;
            Value = default(T);
        }
    }
}