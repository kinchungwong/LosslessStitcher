using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionsUtility
{
    public class ReadOnlyUniqueList<T>
        : IUniqueList<T>
    {
        private IUniqueList<T> _target;

        public int Count => _target.Count;

        public T this[int index] => ItemAt(index);

        public ReadOnlyUniqueList(IUniqueList<T> target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            _target = target;
        }

        public int IndexOf(T t) => _target.IndexOf(t);

        public T ItemAt(int index) => _target.ItemAt(index);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)_target).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_target).GetEnumerator();

        IEnumerator<(int Index, T Item)> IEnumerable<(int Index, T Item)>.GetEnumerator() =>
            ((IEnumerable<(int Index, T Item)>)_target).GetEnumerator();
    }
}
