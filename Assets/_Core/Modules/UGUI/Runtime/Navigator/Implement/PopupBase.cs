using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Kelsey.UGUI
{
    public class PopupBase : View
    {
        [SerializeField] private bool isShowBackDrop = true;
        public bool IsShowBackDrop => isShowBackDrop;

        [SerializeField, ShowIf("isShowBackDrop")]
        private bool isBackDropClickedExitPopup;

        [CanBeNull] PopupBackDrop _backDrop;
        [CanBeNull] public PopupBackDrop BackDrop => _backDrop;

        private AsyncOperationHandle<GameObject> _operationHandle;

        protected override ITransitionAnimation GetDefaultEnterTransitionAnimation() =>
            Service<NavigatorDatabase>.Instance.DefaultPopupEnterAnimation;

        protected override ITransitionAnimation GetDefaultExitTransitionAnimation() =>
            Service<NavigatorDatabase>.Instance.DefaultPopupExitAnimation;

        public void AssignBackDrop(PopupBackDrop backDrop)
        {
            _backDrop = backDrop;
            backDrop.AssignPopup(this);
        }
        
        public void AssignOperationHandle(AsyncOperationHandle<GameObject> operationHandle)
        {
            _operationHandle = operationHandle;
        }

        public override async UniTask Enter()
        {
            if (IsAnimating) return;

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

            BackDrop?.Exit();
            await base.Exit();
            Destroy(gameObject);
            if (BackDrop != null) Destroy(BackDrop.gameObject);
            if (_operationHandle.IsValid()) _operationHandle.Release();
        }

        public void OnBackdropClick()
        {
            if (isBackDropClickedExitPopup) Exit();
        }
    }
}