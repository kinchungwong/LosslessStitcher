using System.Collections;
using System.Collections.Generic;

namespace LosslessStitcher.Imaging.Hash2D.Simple
{
    public class StageList
        : IStageList
    {
        internal List<StageInfo> _list;

        IStageInfo IReadOnlyList<IStageInfo>.this[int index] => _list[index];

        public StageInfo this[int index] => _list[index];

        public int Count => _list.Count;

        public StageList()
        {
            _list = new List<StageInfo>();
        }

        public StageList(IStageList other)
            : this()
        {
            foreach (var stage in other)
            {
                _list.Add(new StageInfo(stage));
            }
        }

        public IEnumerator<IStageInfo> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
