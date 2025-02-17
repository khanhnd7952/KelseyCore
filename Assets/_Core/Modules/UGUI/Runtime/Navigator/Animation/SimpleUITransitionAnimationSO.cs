using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kelsey.UGUI
{
    [CreateAssetMenu(menuName = "Navigator/Simple UI Transition Asset", fileName = "simple_ui_transition_asset.asset")]
    public class SimpleUITransitionAnimationSO : UITransitionAnimationSO
    {
        [SerializeField] private TweenPosition tweenPosition;
        [SerializeField] private TweenScale tweenScale;
        [SerializeField] private TweenAlpha tweenFade;

        public override void SetUp()
        {
            tweenPosition.SetUp(Target);
            tweenScale.SetUp(Target);
            tweenFade.SetUp(Target);
        }

        public override void Prepare()
        {
            tweenPosition.Prepare();
            tweenScale.Prepare();
            tweenFade.Prepare();
        }

        public override async UniTask PlayAsync()
        {
            await UniTask.WhenAll(tweenPosition.PlayAsync(), tweenScale.PlayAsync(), tweenFade.PlayAsync());
        }
    }
}