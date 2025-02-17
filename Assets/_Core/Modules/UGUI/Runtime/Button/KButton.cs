using System;
using DG.Tweening;
using Sisus.Init;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Kelsey.UGUI
{
    public class KButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float clickDelay = 0.5f;
        [SerializeField] private bool scale = true;
        [SerializeField] private bool playSound = true;
        [SerializeField] private bool playVibration = true;

        const float MinScale = 0.9f;
        const float ScaleDuration = 0.15f;
        private Vector3 _originalScale = Vector3.one;
        private bool _isScaling = false;
        float _lastTimeClick;

        protected virtual void OnClick()
        {
            onClick?.Invoke();

            if (playVibration && Service.TryGet<IVibrationService>(out var vibrationService))
            {
                vibrationService.PlayVibrationClick();
            }

            if (playSound && Service.TryGet<IAudioService>(out var audioService))
            {
                audioService.PlaySoundClick();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.unscaledTime < _lastTimeClick + clickDelay) return;
            _lastTimeClick = Time.unscaledTime;
            OnClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!scale) return;
            _isScaling = true;
            transform.DOKill();
            transform.DOScale(new Vector3(MinScale * _originalScale.x, MinScale * _originalScale.y, 1f), ScaleDuration);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isScaling)
            {
                _isScaling = false;
                transform.DOKill();
                transform.DOScale(_originalScale, ScaleDuration);
            }
        }

        public void RegisterOnClick(Action action)
        {
            onClick.AddListener(() => action?.Invoke());
        }

        public void UnRegisterOnClick(Action action)
        {
            onClick.RemoveListener(() => action?.Invoke());
        }


        [SerializeField] UnityEvent onClick = new UnityEvent();
    }
}