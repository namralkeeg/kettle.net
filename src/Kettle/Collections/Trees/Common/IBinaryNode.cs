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

namespace Kettle.Collections.Trees.Common
{
    /// <summary>
    /// Defines methods for implementing a binary node.
    /// </summary>
    /// <typeparam name="T">The type of object to store in the binary node.</typeparam>
    internal interface IBinaryNode<T>
    {
        #region Properties

        /// <summary>
        /// Gets if this node is a leaf node.
        /// </summary>
        bool IsLeaf { get; }

        /// <summary>
        /// Gets and sets reference to the left node.
        /// </summary>
        IBinaryNode<T> Left { get; set; }

        /// <summary>
        /// Gets and sets reference to the right node.
        /// </summary>
        IBinaryNode<T> Right { get; set; }

        /// <summary>
        /// Gets and sets the value to store in the node.
        /// </summary>
        IBinaryNode<T> Value { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Resets everything to their default values.
        /// </summary>
        void Invalidate();

        #endregion Methods
    }
}