using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kelsey.UGUI
{
    [Serializable]
    public abstract class TweenBase
    {
        protected RectTransform Target { get; private set; }

        public virtual void SetUp(RectTransform target)
        {
            Target = target;
        }

        public abstract void Prepare();

        public abstract UniTask PlayAsync();
    }
}