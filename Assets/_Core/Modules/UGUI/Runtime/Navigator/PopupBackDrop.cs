using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(Image))]
    public class PopupBackDrop : MonoBehaviour, IPointerClickHandler
    {
        Image _image;
        float _endAlpha;
        Popup _popup;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _endAlpha = _image.color.a;
        }

        public void AssignPopup(Popup popup)
        {
            _popup = popup;
        }

        public async UniTask Enter()
        {
            await _image.DOFade(_endAlpha, 0.2f).From(0f);
        }

        public async UniTask Exit()
        {
            await _image.DOFade(0f, 0.2f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _popup.OnBackdropClick();
        }
    }
}