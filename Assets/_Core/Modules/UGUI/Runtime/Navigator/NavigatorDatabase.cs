using Sisus.Init;
using UnityEngine;

namespace Kelsey.UGUI
{
    [Service(ResourcePath = CONFIG_NAME)]
    [CreateAssetMenu(fileName = CONFIG_NAME, menuName = BASE_NAME + "/" + CONFIG_NAME)]
    public class NavigatorDatabase : ScriptableObject
    {
        const string BASE_NAME = "UI";
        const string CONFIG_NAME = "NavigatorDatabase";

        [SerializeField] UITransitionAnimationSO defaultPageEnterAnimation;
        [SerializeField] UITransitionAnimationSO defaultPageExitAnimation;

        [SerializeField] UITransitionAnimationSO defaultPopupEnterAnimation;
        [SerializeField] UITransitionAnimationSO defaultPopupExitAnimation;

        [SerializeField] private AudioClip clipPopupEnter, clipPopupExit;


        public UITransitionAnimationSO DefaultPageEnterAnimation => defaultPageEnterAnimation;
        public UITransitionAnimationSO DefaultPageExitAnimation => defaultPageExitAnimation;
        public UITransitionAnimationSO DefaultPopupEnterAnimation => defaultPopupEnterAnimation;
        public UITransitionAnimationSO DefaultPopupExitAnimation => defaultPopupExitAnimation;
        
        public AudioClip ClipPopupEnter => clipPopupEnter;
        public AudioClip ClipPopupExit => clipPopupExit;
    }
}