using Febucci.UI;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Squirrel.FeelExtension.TextAnimator
{
    [AddComponentMenu("")]
    [FeedbackHelp("You can add a description for your feedback here.")]
    [FeedbackPath("TextAnimator/Disappear")]
    public class MMF_TextAnimator_Disappear : MMF_Feedback
    {
        [MMFInspectorGroup("Target", true, 12, true)]
        public TypewriterByCharacter TargetTypewriter;
        
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            TargetTypewriter.StartDisappearingText();
        }
    }
}