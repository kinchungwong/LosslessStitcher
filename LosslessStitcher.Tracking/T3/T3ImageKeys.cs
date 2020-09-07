using System;
using System.Collections.Generic;
using CollectionsUtility;

namespace LosslessStitcher.Tracking.T3
{
    using System.Collections;

    public sealed class T3ImageKeys
        : ReadOnlyUniqueList<int>
    {
        public T3ImageKeys(IEnumerable<int> imageKeys)
            : base(_CtorCreateWithValidate(imageKeys))
        {
        }

        private static UniqueList<int> _CtorCreateWithValidate(IEnumerable<int> imageKeys)
        {
            if (imageKeys is null)
            {
                throw new ArgumentNullException(nameof(imageKeys));
            }
            var uniqueList = new UniqueList<int>(imageKeys);
            if (uniqueList.Count != 3)
            {
                throw new ArgumentException(paramName: nameof(imageKeys),
                    message: "Argument must contain exactly three distinct image keys.");
            }
            return uniqueList;
        }
    }
}
