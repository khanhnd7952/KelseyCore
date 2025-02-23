using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kelsey.Extension;
using Kelsey.SerializeInterface;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace Kelsey.UGUI
{
    public abstract class View : MonoBehaviour
    {
        private const string ResourcePath = "View/";

        [SerializeField] private InterfaceReference<ITransitionAnimation>[] enterAnimations;
        [SerializeField] private InterfaceReference<ITransitionAnimation>[] exitAnimations;

        [ShowInInspector, ReadOnly] protected bool IsAnimating { get; private set; }
        [ShowInInspector, ReadOnly] protected bool IsShowing { get; private set; }

        private bool _isInitialized;

        public async UniTask InitializeAsync()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            PlaySoundOpen();
            await Initialize();
        }

        protected virtual UniTask Initialize() => UniTask.CompletedTask;

        List<UniTask> transitionTasks = new List<UniTask>();
        protected abstract ITransitionAnimation GetDefaultEnterTransitionAnimation();
        protected abstract ITransitionAnimation GetDefaultExitTransitionAnimation();


        [Button]
        public virtual async UniTask Enter()
        {
            if (IsAnimating) return;
            IsShowing = true;
            IsAnimating = true;
            transitionTasks.Clear();
            if (!enterAnimations.KIsNullOrMT())
            {
                foreach (var transitionAnimation in enterAnimations)
                {
                    transitionTasks.Add(PlayAnim(transitionAnimation.Value));
                }
            }
            else
            {
                var defaultEnterTransitionAnimation = GetDefaultEnterTransitionAnimation();
                if (defaultEnterTransitionAnimation != null)
                    transitionTasks.Add(PlayAnim(defaultEnterTransitionAnimation));
            }

            await UniTask.WhenAll(transitionTasks);
            IsAnimating = false;
        }

        [Button]
        public virtual async UniTask Exit()
        {
            if (IsAnimating) return;
            IsAnimating = true;
            transitionTasks.Clear();
            if (!exitAnimations.KIsNullOrMT())
            {
                foreach (var transitionAnimation in exitAnimations)
                {
                    transitionTasks.Add(PlayAnim(transitionAnimation.Value));
                }
            }
            else
            {
                var defaultExitTransitionAnimation = GetDefaultExitTransitionAnimation();
                if (defaultExitTransitionAnimation != null)
                    transitionTasks.Add(PlayAnim(defaultExitTransitionAnimation));
            }

            await UniTask.WhenAll(transitionTasks);
            IsAnimating = false;
            IsShowing = false;
        }

        async UniTask PlayAnim(ITransitionAnimation anim)
        {
            try
            {
                if (anim == null) return;
                anim.SetUp(transform as RectTransform);
                anim.Prepare();
                await anim.PlayAsync();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        protected virtual void PlaySoundOpen()
        {
            //if (enabledSound) audioOpen.Play();
        }

        protected virtual void PlaySoundClose()
        {
            //if (enabledSound) audioClose.Play();
        }

        #region Static Method

        public static T GetView<T>(string key = "") where T : View
        {
            if (string.IsNullOrEmpty(key)) key = typeof(T).Name;
            var view = Resources.Load<T>(ResourcePath + key);
            return view;
        }

        public static View[] GetViews()
        {
            var views = Resources.LoadAll<View>(ResourcePath);
            return views;
        }

        #endregion

#if UNITY_EDITOR
        [Button]
        void Rename()
        {
            name = GetType().Name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        public UniTask WaitForClose()
        {
            return UniTask.WaitUntil(() => !IsShowing);
        }
    }
}