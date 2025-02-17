using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kelsey.UGUI
{
    [Serializable]
    public class TransitionAnimation
    {


        [SerializeField] private EAnimationAssetType type;

        [SerializeField] [ShowIf("IsAssetTypeIsMono")]
        private UITransitionComponent animMono;

        [SerializeField] [ShowIf("IsAssetTypeIsSo")]
        private UITransitionAnimationSO animScriptable;

#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        private bool IsAssetTypeIsMono => type == EAnimationAssetType.MonoBehaviour;

        // ReSharper disable once UnusedMember.Local
        private bool IsAssetTypeIsSo => type == EAnimationAssetType.ScriptableObject;
#endif

        public ITransitionAnimation GetAnimation()
        {
            return type switch
            {
                EAnimationAssetType.MonoBehaviour => animMono,
                EAnimationAssetType.ScriptableObject => animScriptable,
                _ => null
            };
        }
    }
}