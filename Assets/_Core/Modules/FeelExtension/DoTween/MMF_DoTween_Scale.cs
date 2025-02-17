using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Squirrel.FeelExtension.DoTween
{
    [AddComponentMenu("")]
    [FeedbackPath("DoTween/Scale")]
    public class MMF_DoTween_Scale : MMF_Pause
    {
        [MMFInspectorGroup("Target")] [SerializeField]
        private Transform target;

        [MMFInspectorGroup("Target")] [SerializeField]
        private Vector3 from = Vector3.zero;

        [MMFInspectorGroup("Target")] [SerializeField]
        private Vector3 to = Vector3.one;

        [MMFInspectorGroup("Target")] [SerializeField]
        private float duration = 0.2f;

        [MMFInspectorGroup("Target")] [SerializeField]
        Ease ease = Ease.Linear;

        public override float FeedbackDuration => duration;

        protected override void CustomInitialization(MMF_Player owner)
        {
            base.CustomInitialization(owner);
            PauseDuration = duration;
        }

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (target == null)
            {
                Debug.LogWarning("No target set for this MMF_DoTween_Scale. Aborting.");
                return;
            }

            target.transform.DOKill();
            target.transform.DOScale(to, duration).From(from).SetEase(ease);
            base.CustomPlayFeedback(position, feedbacksIntensity);
        }
    }
}