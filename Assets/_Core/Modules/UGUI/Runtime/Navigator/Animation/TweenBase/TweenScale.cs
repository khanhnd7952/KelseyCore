using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Kelsey.UGUI
{
    [Serializable]
    public class TweenScale : TweenTarget<Vector3>
    {
        public override void Prepare()
        {
            if (!isUse) return;
            //Assert.NotNull(Target);
            Target.localScale = beforeValue;
        }

        protected override async UniTask Play()
        {
            await Target.DOScale(afterValue, duration).SetEase(ease);
        }
    }
}