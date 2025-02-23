using System;
using DG.Tweening;
using Sisus.Init;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kelsey.UGUI
{
    public class KButton : MonoBehaviour<IVibrationService, IAudioService>, IPointerClickHandler, IPointerDownHandler,
        IPointerUpHandler
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
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.unscaledTime < _lastTimeClick + clickDelay) return;
            _lastTimeClick = Time.unscaledTime;

            if (playVibration) vibrationService.PlayVibrationClick();

            if (playSound) audioService.PlaySoundClick();
            OnClick();
            _onClick?.Invoke();
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

        public void RegisterOnClick(Action action) => _onClick += action;

        public void UnRegisterOnClick(Action action) => _onClick -= action;


        Action _onClick;

        IVibrationService vibrationService;
        IAudioService audioService;

        protected override void Init(IVibrationService firstArgument, IAudioService secondArgument)
        {
            vibrationService = firstArgument;
            audioService = secondArgument;
        }
    }
}