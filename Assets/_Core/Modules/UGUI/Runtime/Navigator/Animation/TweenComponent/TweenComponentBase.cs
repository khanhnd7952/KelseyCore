using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kelsey.UGUI
{
    public abstract class TweenComponentBase<TTweenBase> : MonoBehaviour, ITransitionAnimation where TTweenBase : TweenBase
    {
        [SerializeField] private TTweenBase tween;

        public void SetUp(RectTransform rectTransform)
        {
            tween.SetUp(transform as RectTransform);
        }

        public void Prepare()
        {
            tween.Prepare();
        }

        public UniTask PlayAsync()
        {
            return tween.PlayAsync();
        }
    }
}