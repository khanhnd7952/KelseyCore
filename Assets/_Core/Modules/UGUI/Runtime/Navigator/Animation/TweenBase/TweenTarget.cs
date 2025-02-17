using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Kelsey.UGUI
{
    [Serializable]
    public abstract class TweenTarget<T> : TweenBase
    {
        [SerializeField] protected bool isUse;
        [SerializeField] protected T beforeValue;
        [SerializeField] protected T afterValue;
        [SerializeField] protected Ease ease = Ease.Linear;
        [SerializeField] protected float delay;
        [SerializeField] protected float duration;

        public override async UniTask PlayAsync()
        {
            if (!isUse) return;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            await Play();
        }

        protected abstract UniTask Play();


        public bool IsUse() => isUse;
    }
}