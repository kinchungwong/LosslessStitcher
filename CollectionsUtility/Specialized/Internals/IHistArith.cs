using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionsUtility.Specialized.Internals
{
    public interface IHistArith<T>
        : IComparer<T>
        where T : struct
    {
        T Zero { get; }

        T One { get; }

        T Add(T arg0, T arg1);
    }
}
