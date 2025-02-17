using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sisus.Init;
using UnityEngine;
using UnityEngine.UI;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(RectMask2D))]
    [Service(typeof(IPopupContainer), FindFromScene = true, LazyInit = true)]
    class PopupContainer : MonoBehaviour, IPopupContainer
    {
        [SerializeField] private PopupBackDrop backDropPrefab;
        List<Popup> _recyclePopups = new();

        public T Push<T>(string key = "") where T : Popup
        {
            // instantiate popup
            var popupInstance = InstantiatePopup<T>(key);
            CheckRecyclePopup(popupInstance);

            // anim enter popup
            popupInstance.Enter();
            return popupInstance;
        }

        public async UniTask<T> PushAsync<T>(string key = "") where T : Popup
        {
            // instantiate popup
            var popupInstance = InstantiatePopup<T>(key);
            CheckRecyclePopup(popupInstance);

            // anim enter popup
            await popupInstance.Enter();
            return popupInstance;
        }

        T InstantiatePopup<T>(string key) where T : Popup
        {
            // instantiate backdrop
            var popup = GetPopup<T>(key);

            // bring popup to front
            popup.BackDrop?.transform.SetAsLastSibling();
            popup.transform.SetAsLastSibling();

            return popup;
        }

        T GetPopup<T>(string key) where T : Popup
        {
            if (GetRecyclePopup<T>() != null) return GetRecyclePopup<T>();
            var prefab = View.GetView<T>(key);
            var popupInstance = Instantiate(prefab, transform);
            if (popupInstance.IsShowBackDrop)
            {
                var backDrop = Instantiate(backDropPrefab, transform);

                // assign popup and backdrop
                popupInstance.AssignBackDrop(backDrop);
            }

            return popupInstance;
        }

        T GetRecyclePopup<T>() where T : Popup
        {
            foreach (var recyclePopup in _recyclePopups)
            {
                if (recyclePopup.GetType() == typeof(T))
                {
                    return (T)recyclePopup;
                }
            }

            return null;
        }

        void CheckRecyclePopup(Popup popup)
        {
            if (popup.IsRecyclePopup && !_recyclePopups.Contains(popup))
            {
                _recyclePopups.Add(popup);
            }
        }

        //public T GetPopup<T>() where T : Popup => View.GetView<T>();
    }
}