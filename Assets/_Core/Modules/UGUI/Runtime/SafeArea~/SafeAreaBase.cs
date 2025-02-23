﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Squirrel.UGUI
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RuntimeSafeAreaUpdater))]
    public abstract class SafeAreaBase : MonoBehaviour, ISafeAreaUpdatable
    {
        private RectTransform _rectTransform;

        protected RectTransform RectT
        {
            get
            {
#if UNITY_EDITOR
                try
                {
#endif
                    return _rectTransform != null ? _rectTransform : _rectTransform = GetComponent<RectTransform>();
#if UNITY_EDITOR
                }
                catch (Exception)
                {
                    return null;
                }
#endif
            }
        }


        protected void Reset()
        {
            ResetRect();
            UpdateRect();
        }

        public virtual void ResetRect()
        {
            var center = new Vector2(0.5f, 0.5f);
            RectT.sizeDelta = Vector2.zero;
            RectT.anchoredPosition = Vector2.zero;
            RectT.anchorMin = center;
            RectT.anchorMax = center;
            RectT.pivot = center;
            RectT.localRotation = Quaternion.identity;
            RectT.localScale = Vector3.one;
        }

        public virtual void UpdateRect()
        {
            int width;
            int height;
#if UNITY_EDITOR
            width = ShimManagerProxy.Width;
            height = ShimManagerProxy.Height;
#else
            width = UnityEngine.Screen.width;
            height = UnityEngine.Screen.height;
#endif
            UpdateRect(Screen.safeArea, width, height);
        }

        protected abstract void UpdateRect(Rect safeArea, int width, int height);

        private void Start()
        {
#if UNITY_EDITOR
            SetDirty();
#else
            UpdateRect();
#endif
        }


#if UNITY_EDITOR
        private bool _isDirty;
        private void LockRect() { RectT.hideFlags = HideFlags.NotEditable; }

        private void UnlockRect() { RectT.hideFlags = HideFlags.None; }

        protected void OnEnable()
        {
            SimulatorWindowEvent.OnOpen += SetDirty;
            SimulatorWindowEvent.OnClose += SetDirty;
            SimulatorWindowEvent.OnOrientationChanged += OnOrientationChanged;
            ShimManagerEvent.OnActiveShimChanged += SetDirty;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += OnSceneSaving;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += OnSceneSaved;
            UnityEditor.SceneManagement.PrefabStage.prefabSaving += OnPrefabSaving;
            UnityEditor.SceneManagement.PrefabStage.prefabSaved += OnPrefabSaved;

            LockRect();
            TryUpdateRect();
        }

        protected void OnDisable()
        {
            SimulatorWindowEvent.OnOpen -= SetDirty;
            SimulatorWindowEvent.OnClose -= SetDirty;
            SimulatorWindowEvent.OnOrientationChanged -= OnOrientationChanged;
            ShimManagerEvent.OnActiveShimChanged -= SetDirty;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving -= OnSceneSaving;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaved -= OnSceneSaved;
            UnityEditor.SceneManagement.PrefabStage.prefabSaving -= OnPrefabSaving;
            UnityEditor.SceneManagement.PrefabStage.prefabSaved -= OnPrefabSaved;

            UnlockRect();
        }

        private void OnOrientationChanged(ScreenOrientation orientation)
        {
            if (UnityEditor.EditorApplication.isPlaying == false) SetDirty();
        }

        private void OnSceneSaving(Scene scene, string path) => TryResetRect();

        private void OnSceneSaved(Scene scene) => TryUpdateRect();

        private void OnPrefabSaving(GameObject prefabContentsRoot) => TryResetRect();

        private void OnPrefabSaved(GameObject prefabContentsRoot) => TryUpdateRect();

        private void TryResetRect()
        {
            if (RectT) ResetRect();
        }

        private void TryUpdateRect()
        {
            if (RectT) UpdateRect();
        }

        private void SetDirty() => _isDirty = true;

        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlaying == false) SetDirty();
        }

        private void OnGUI()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (_isDirty == false) return;

                _isDirty = false;
                TryUpdateRect();

                if (UnityEditor.EditorApplication.isPlaying == false)
                {
#if UNITY_2021_1
                    SafeAreEditor.SimulatorWindowProxy.Repaint();
#else
                    SimulatorWindowProxy.RepaintWithDelay();
#endif
                }
            };
        }

        [Button, DisableInEditorMode]
        private void ForceUpdateRect()
        {
            UpdateRect();
            SimulatorWindowProxy.Repaint();
        }
#endif
    }
}