using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Kelsey.UGUI
{
    [Serializable]
    public class TweenPosition : TweenTarget<Vector3>
    {
        [SerializeField] private EAlignment beforeAlignment;
        [SerializeField] private EAlignment afterAlignment;

        private Vector3 _afterPosition;
        private Vector3 _beforePosition;

        public override void Prepare()
        {
            if(!isUse) return;

            //Assert.NotNull(Target);

            _beforePosition = beforeAlignment.ToPosition(Target);
            _afterPosition = afterAlignment.ToPosition(Target);

            Target.localPosition = _beforePosition;
        }

        protected override async UniTask Play()
        {
            await Target.DOLocalMove(_afterPosition, duration).SetEase(ease);
        }

    }
}