using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace Kelsey.UGUI
{
    public class Popup : View
    {
        [TitleGroup("Sound")] [SerializeField] private bool playSoundEnter = false;
        [TitleGroup("Sound")] [SerializeField] private bool playSoundExit = false;

        [SerializeField] private bool recyclePopup = false;
        public bool IsRecyclePopup => recyclePopup;

        [SerializeField] private bool isShowBackDrop = true;
        public bool IsShowBackDrop => isShowBackDrop;

        [SerializeField, ShowIf("isShowBackDrop")]
        private bool isBackDropClickedExitPopup;

        [CanBeNull] PopupBackDrop _backDrop;
        [CanBeNull] public PopupBackDrop BackDrop => _backDrop;

        public void AssignBackDrop(PopupBackDrop backDrop)
        {
            _backDrop = backDrop;
            backDrop.AssignPopup(this);
        }

        protected override ITransitionAnimation GetDefaultEnterTransitionAnimation() => Service<NavigatorDatabase>.Instance.DefaultPopupEnterAnimation;
        protected override ITransitionAnimation GetDefaultExitTransitionAnimation() => Service<NavigatorDatabase>.Instance.DefaultPopupExitAnimation;

        public override async UniTask Enter()
        {
            if (IsAnimating) return;
            if (playSoundEnter)
            {
                if (Service.TryGet<IAudioService>(out var audioService) && Service.TryGet<NavigatorDatabase>(out var navigatorDatabase))
                {
                    audioService.PlaySoundFx(navigatorDatabase.ClipPopupEnter);
                }
            }

            if (BackDrop != null)
            {
                BackDrop.gameObject.SetActive(true);
                BackDrop.Enter();
            }

            gameObject.SetActive(true);
            await base.Enter();
        }

        public override async UniTask Exit()
        {
            if (IsAnimating) return;
            if (playSoundExit)
            {
                if (Service.TryGet<IAudioService>(out var audioService) && Service.TryGet<NavigatorDatabase>(out var navigatorDatabase))
                {
                    audioService.PlaySoundFx(navigatorDatabase.ClipPopupExit);
                }
            }

            BackDrop?.Exit();
            await base.Exit();
            if (!recyclePopup)
            {
                Destroy(gameObject);
                if (BackDrop != null) Destroy(BackDrop.gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                BackDrop?.gameObject.SetActive(false);
            }
        }

        public virtual void OnBackdropClick()
        {
            if (isBackDropClickedExitPopup) Exit();
        }
    }
}