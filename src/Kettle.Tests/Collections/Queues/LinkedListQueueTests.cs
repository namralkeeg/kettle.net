using System;
using Kettle.Collections.Queues;
using Xunit;

namespace Kettle.Tests.Collections.Queues
{
    public class LinkedListQueueTests
    {
        public class EmptyLinkedListQueue
        {
            private readonly IQueue<int> queue;

            public EmptyLinkedListQueue()
            {
                queue = new LinkedListQueue<int>();
            }

            [Fact]
            public void Count_ShouldReturnZero()
            {
                int queueCount = queue.Count;
                Assert.Equal(0, queueCount);
            }

            [Fact]
            public void Contains_ShouldReturnFalse()
            {
                bool contains = queue.Contains(10);
                Assert.False(contains);
            }

            [Fact]
            public void Dequeue_ShouldThrowInvalidOperationException()
            {
                Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
            }

            [Fact]
            public void Enqueue_CountShouldBeOne()
            {
                queue.Enqueue(10);
                Assert.Equal(1, queue.Count);
            }

            [Fact]
            public void Peek_ShouldThrowInvalidOperationException()
            {
                Assert.Throws<InvalidOperationException>(() => queue.Peek());
            }

            [Fact]
            public void Remove_ShouldThrowNotImplementedException()
            {
                Assert.Throws<NotImplementedException>(() => queue.Remove(10));
            }
        }

        public class LinkedListQueueOneElement
        {
            private const int oneValue = 42;
            private readonly LinkedListQueue<int> queue;

            public LinkedListQueueOneElement()
            {
                queue = new LinkedListQueue<int>()
                {
                    oneValue
                };
            }

            [Fact]
            public void Count_ShouldBeOne()
            {
                Assert.Single(queue);
            }

            [Fact]
            public void Contains_ShouldBeTrue()
            {
                bool contains = queue.Contains(oneValue);
                Assert.True(contains);
            }

            [Fact]
            public void Dequeue_ShouldBeOneValue()
            {
                Assert.Equal(oneValue, queue.Dequeue());
            }

            [Fact]
            public void Enqueue_CountShouldBeTwo()
            {
                queue.Enqueue(10);
                Assert.Equal(2, queue.Count);
            }

            [Fact]
            public void Peek_ShouleBeOneValue()
            {
                Assert.Equal(oneValue, queue.Peek());
            }

            [Fact]
            public void TryDequeue_ShouleBeOneValue()
            {
                Assert.True(queue.TryDequeue(out int result));
                Assert.Equal(oneValue, result);
            }

            [Fact]
            public void TryDequeue_ShouleBeTrue()
            {
                Assert.True(queue.TryDequeue(out int result));
            }

            [Fact]
            public void TryPeek_ShouldBeOneValue()
            {
                Assert.True(queue.TryPeek(out int result));
                Assert.Equal(oneValue, result);
            }

            [Fact]
            public void TryPeek_ShouleBeTrue()
            {
                Assert.True(queue.TryPeek(out int result));
            }
        }

        public class LinkedListQueueMultipleValues
        {
            private static readonly int[] unsortedArray = { 13, 10, 15, 16, 5, 11, 4, 6, 14, 3, 7 };
            private readonly LinkedListQueue<int> queue;

            public LinkedListQueueMultipleValues()
            {
                queue = new LinkedListQueue<int>(unsortedArray);
            }

            [Fact]
            public void Count_ShouldBeEleven()
            {
                Assert.Equal(11, queue.Count);
            }

            [Fact]
            public void Contains_ShouldBeTrue()
            {
                bool contains = queue.Contains(unsortedArray[10]);
                Assert.True(contains);
            }

            [Fact]
            public void Contains_EnumerableShouldBeTrue()
            {
                Assert.Contains(unsortedArray[10], queue);
            }

            [Fact]
            public void Dequeue_ShouldBe13()
            {
                Assert.Equal(13, queue.Dequeue());
            }

            [Fact]
            public void Dequeue_VerifyDequeueOrder()
            {
                int item;
                int count = queue.Count;
                for (int i = 0; i < count; i++)
                {
                    item = queue.Dequeue();
                    Assert.Equal(unsortedArray[i], item);
                }
            }

            [Fact]
            public void Enumerate_VerifyEnqueueOrder()
            {
                int i = 0;
                foreach (var item in queue)
                {
                    Assert.Equal(unsortedArray[i++], item);
                }
            }

            [Fact]
            public void Peek_ShouldBe13()
            {
                Assert.Equal(13, queue.Peek());
            }
        }
    }
}