using Cysharp.Threading.Tasks;
using Sisus.Init;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(RectMask2D))]
    [Service(typeof(IPopupContainer), FindFromScene = true, LazyInit = true)]
    class PopupContainer : MonoBehaviour, IPopupContainer
    {
        [SerializeField] private PopupBackDrop backDropPrefab;

        async UniTask<TPopup> InstantiatePopup<TPopup>(string key) where TPopup : PopupBase
        {
            // instantiate backdrop
            var popup = await GetPopup<TPopup>(key);

            // bring popup to front
            popup.BackDrop?.transform.SetAsLastSibling();
            popup.transform.SetAsLastSibling();

            return popup;
        }

        async UniTask<TPopup> GetPopup<TPopup>(string key) where TPopup : PopupBase
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(key);
            await asyncOperationHandle;
            var popupPrefab = asyncOperationHandle.Result.GetComponent<TPopup>();
            var popupInstance = Instantiate(popupPrefab, transform);
            popupInstance.gameObject.SetActive(false);
            popupInstance.AssignOperationHandle(asyncOperationHandle);

            if (popupInstance.IsShowBackDrop)
            {
                var backDrop = Instantiate(backDropPrefab, transform);
                backDrop.gameObject.SetActive(false);
                // assign popup and backdrop
                popupInstance.AssignBackDrop(backDrop);
            }

            return popupInstance;
        }

        public async UniTask<TPopup> Push<TPopup>(string key) where TPopup : PopupBase
        {
            var popup = await InstantiatePopup<TPopup>(key);
            await popup.Enter();
            return popup;
        }

        public async UniTask<TPopup> Push<TPopup, TData>(string key, TData data)
            where TPopup : Popup<TData>
        {
            var popup = await InstantiatePopup<TPopup>(key);
            popup.SetPopupData(data);
            await popup.Enter();
            return popup;
        }
    }
}