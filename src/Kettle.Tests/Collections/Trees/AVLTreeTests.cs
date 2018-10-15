using Kettle.Collections.Trees;
using Xunit;

namespace Kettle.Tests.Collections.Trees
{
    public class AVLTreeTests
    {
        public class EmptyAVLTree
        {
            private readonly AVLTree<int> tree;

            public EmptyAVLTree()
            {
                tree = new AVLTree<int>();
            }

            [Fact]
            public void Count_ShouldReturnZero()
            {
                int treeCount = tree.Count;
                Assert.Equal(0, treeCount);
            }

            [Fact]
            public void Contains_ShouldReturnFalse()
            {
                bool contains = tree.Contains(10);
                Assert.False(contains);
            }

            [Fact]
            public void Remove_ShouldReturnFalse()
            {
                bool removed = tree.Remove(10);
                Assert.False(removed);
            }

            [Fact]
            public void Height_ShouldBeNegative()
            {
                Assert.Equal(-1, tree.Height);
            }

            [Fact]
            public void LeafCount_ShouldBeZero()
            {
                Assert.Equal(0, tree.LeafCount);
            }

            [Fact]
            public void Width_ShouldBeZero()
            {
                Assert.Equal(0, tree.Width);
            }
        }

        public class AVLTreeOneElement
        {
            private const int oneValue = 42;
            private readonly AVLTree<int> tree;

            public AVLTreeOneElement()
            {
                tree = new AVLTree<int>
                {
                    oneValue
                };
            }

            [Fact]
            public void Count_ShouldBeOne()
            {
                Assert.Single(tree);
            }

            [Fact]
            public void Contains_ShouldBeTrue()
            {
                bool contains = tree.Contains(oneValue);
                Assert.True(contains);
            }

            [Fact]
            public void Remove_ShouldReturnTrue()
            {
                bool removed = tree.Remove(oneValue);
                Assert.True(removed);
            }

            [Fact]
            public void Remove_CountShouldBeZero()
            {
                tree.Remove(oneValue);
                Assert.Empty(tree);
            }

            [Fact]
            public void Height_ShouldBeZero()
            {
                Assert.Equal(0, tree.Height);
            }

            [Fact]
            public void LeafCount_ShouldBeOne()
            {
                Assert.Equal(1, tree.LeafCount);
            }

            [Fact]
            public void Width_ShouldBeZero()
            {
                Assert.Equal(0, tree.Width);
            }
        }

        public class AVLTreeMultipleValues
        {
            private static readonly int[] unsortedArray = { 13, 10, 15, 16, 5, 11, 4, 6, 14, 3, 7 };
            private static readonly int[] sortedArray = { 3, 4, 5, 6, 7, 10, 11, 13, 14, 15, 16 };
            private readonly AVLTree<int> tree;

            public AVLTreeMultipleValues()
            {
                tree = new AVLTree<int>(unsortedArray);
            }

            [Fact]
            public void Count_ShouldBeEleven()
            {
                Assert.Equal(11, tree.Count);
            }

            [Fact]
            public void Contains_ShouldBeTrue()
            {
                bool contains = tree.Contains(unsortedArray[10]);
                Assert.True(contains);
            }

            [Fact]
            public void Contains_EnumerableShouldBeTrue()
            {
                Assert.Contains(unsortedArray[10], tree);
            }

            [Fact]
            public void Remove_CountShouldBeTen()
            {
                tree.Remove(unsortedArray[7]);
                Assert.Equal(10, tree.Count);
            }

            [Fact]
            public void Height_ShouldBeThree()
            {
                Assert.Equal(3, tree.Height);
            }

            [Fact]
            public void LeafCount_ShouldBeFive()
            {
                Assert.Equal(5, tree.LeafCount);
            }

            [Fact]
            public void Width_ShouldBeFour()
            {
                Assert.Equal(4, tree.Width);
            }

            [Fact]
            public void Enumerate_VerifyInOrder()
            {
                int i = 0;
                foreach (var item in tree)
                {
                    Assert.Equal(sortedArray[i++], item);
                }
            }

            [Fact]
            public void Enumerate_VerifyReverseOrder()
            {
                int i = tree.Count - 1;
                using (var enumerator = tree.GetReverseEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Assert.Equal(sortedArray[i--], enumerator.Current);
                    }
                }
            }
        }
    }
}