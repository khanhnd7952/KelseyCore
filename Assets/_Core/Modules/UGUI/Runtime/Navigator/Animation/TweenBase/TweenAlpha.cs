using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Kelsey.UGUI
{
    [Serializable]
    public class TweenAlpha : TweenTarget<float>
    {
        private CanvasGroup _canvasGroup;

        public override void Prepare()
        {
            if(!isUse) return;
            //Assert.NotNull(Target);
            if (!Target.gameObject.TryGetComponent(out _canvasGroup)) _canvasGroup = Target.gameObject.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = beforeValue;
        }

        protected override async UniTask Play()
        {
            await _canvasGroup.DOFade(afterValue, duration).SetEase(ease);
        }
    }
}