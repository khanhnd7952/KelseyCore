using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(MMF_Player))]
    public class MMFeedbackAnimationTransition : MonoBehaviour, ITransitionAnimation
    {
        MMF_Player _player;

        private void Awake()
        {
            _player = GetComponent<MMF_Player>();
        }

        public void SetUp(RectTransform rectTransform)
        {
        }

        public void Prepare()
        {
        }

        public async UniTask PlayAsync()
        {
            await _player.PlayFeedbacksTask();
        }
    }
}