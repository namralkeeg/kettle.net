using System.Collections;
using System.Collections.Generic;

namespace Kettle.Collections.Queues
{
    public interface IQueue<T> : ICollection<T>, ICollection, IEnumerable<T>, IReadOnlyCollection<T>
    {
        new int Count { get; }

        T Dequeue();

        void Enqueue(T item);

        T Peek();

        T[] ToArray();

        List<T> ToList();

        bool TryDequeue(out T result);

        bool TryPeek(out T result);
    }
}