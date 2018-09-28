using System;

namespace Kettle.Sorting
{
    /// <summary>
    /// Provides a base class for implementing the <see cref="IKeyedSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort.</typeparam>
    /// <remarks>
    /// Key based sorting is a non-comparison based sort, and instead relies on a key function.
    /// </remarks>
    public abstract class KeyedSort<T> : SortBase<T>, IKeyedSort<T>
    {
        #region Fields

        protected Func<T, int> _keyFunction;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="KeyedSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the key function to <see cref="object.GetHashCode"/>.
        /// </remarks>
        protected KeyedSort() : this((T k) => k.GetHashCode())
        {
        }

        /// <summary>
        /// Initializes a <see cref="KeyedSort{T}"/> class.
        /// </summary>
        /// <param name="keyFunction">
        /// A function <see cref="Func{T, int}"/> that returns an int key based on the value of T.
        /// </param>
        protected KeyedSort(Func<T, int> keyFunction)
        {
            KeyFunction = keyFunction;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Func{T, int}"/> to use for generating keys.
        /// </summary>
        /// <remarks>Defaults to <see cref="object.GetHashCode"/> for key generation if null.</remarks>
        public virtual Func<T, int> KeyFunction
        {
            get => _keyFunction;
            set
            {
                _keyFunction = value ?? ((T k) => k.GetHashCode());
            }
        }

        #endregion Properties
    }
}