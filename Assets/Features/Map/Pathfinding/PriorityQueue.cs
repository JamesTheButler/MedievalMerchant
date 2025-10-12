using System;
using System.Collections.Generic;

namespace Features.Map.Pathfinding
{
    public class PriorityQueue<T, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly List<(T item, TPriority prio)> _items = new();
        public int Count => _items.Count;

        public void Enqueue(T item, TPriority priority) => _items.Add((item, priority));

        public T Dequeue()
        {
            var best = 0;
            for (var i = 1; i < _items.Count; i++)
            {
                if (_items[i].prio.CompareTo(_items[best].prio) < 0)
                {
                    best = i;
                }
            }

            var item = _items[best].item;
            _items.RemoveAt(best);
            return item;
        }

        public void EnqueueOrUpdate(T item, TPriority priority)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(_items[i].item, item))
                    continue;

                _items[i] = (item, priority);
                return;
            }

            Enqueue(item, priority);
        }
    }
}