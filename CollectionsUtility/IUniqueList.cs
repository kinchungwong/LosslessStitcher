using System.Collections.Generic;

namespace CollectionsUtility
{
    public interface IUniqueList<T>
        : IReadOnlyList<T>
        , IEnumerable<(int Index, T Item)>
    {
        int IndexOf(T t);

        T ItemAt(int index);
    }
}
