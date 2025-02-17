using Febucci.UI;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Squirrel.FeelExtension.TextAnimator
{
    [AddComponentMenu("")]
    [FeedbackHelp("You can add a description for your feedback here.")]
    [FeedbackPath("TextAnimator/Appear")]
    public class MMF_TextAnimator_Appear : MMF_Feedback
    {
        [MMFInspectorGroup("Target", true, 12, true)]
        public TypewriterByCharacter TargetTypewriter;
        public string text;
        protected override void CustomInitialization(MMF_Player owner)
        {
            base.CustomInitialization(owner);
            TargetTypewriter.ShowText("");
        }

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            TargetTypewriter.ShowText(text);
            TargetTypewriter.StartShowingText(true);
        }
    }
}