using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    internal class PriorityQueue<P, V>
    {
        private readonly SortedDictionary<P, Queue<V>> _list = new SortedDictionary<P, Queue<V>>();

        public void Enqueue(P priority, V value)
        {
            Queue<V> q;
            if (!_list.TryGetValue(priority, out q))
            {
                q = new Queue<V>();
                _list.Add(priority, q);
            }
            q.Enqueue(value);
        }

        public V Dequeue()
        {
            // will throw if there isn’t any first element!
            var pair = _list.First();
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                _list.Remove(pair.Key);
            return v;
        }

        public bool IsEmpty
        {
            get { return !_list.Any(); }
        }
    }
}
