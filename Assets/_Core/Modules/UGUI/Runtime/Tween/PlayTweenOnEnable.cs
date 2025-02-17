using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

namespace Kelsey.UGUI
{
    public class PlayTweenOnEnable : MonoBehaviour
    {
        [SerializeField] private string tweenId = "showOnEnable";

        private DOTweenAnimation[] _tweeners;

        private void OnEnable()
        {
            if (_tweeners.IsNullOrEmpty())
            {
                _tweeners = GetComponentsInChildren<DOTweenAnimation>(true);
                foreach (DOTweenAnimation tweener in _tweeners)
                {
                    if (tweener.id != tweenId) continue;
                    tweener.CreateTween();
                }
            }

            //Debug.Log(_tweeners.Length);
            if (_tweeners.IsNullOrEmpty()) return;
            foreach (DOTweenAnimation tweener in _tweeners)
            {
                if (tweener.id != tweenId) continue;

                tweener.DORestart();
                tweener.DOPlay();
            }
        }
    }
}