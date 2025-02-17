using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kelsey.UGUI
{
    public abstract class UITransitionAnimationSO : ScriptableObject, ITransitionAnimation
    {
        [ShowInInspector, ReadOnly] public RectTransform Target { get; private set; }

        public void SetUp(RectTransform rectTransform)
        {
            Target = rectTransform;
            SetUp();
        }

        public abstract void Prepare();

        public abstract void SetUp();

        public abstract UniTask PlayAsync();
    }
}