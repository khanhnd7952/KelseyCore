using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kelsey.Extension;
using UnityEngine;

namespace Kelsey.UGUI
{
    public class CoinCollector : MonoBehaviour
    {
        Transform _target;
        float _curveHeigh;
        private float _delayMove;

        public void SetUp(Transform target, float curveHeigh, float delayMove)
        {
            _target = target;
            _curveHeigh = curveHeigh;
            _delayMove = delayMove;
        }

        public void Spawn()
        {
            transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).From(0);

            var randomDurationMoveY = UnityEngine.Random.Range(0.4f, 1f);
            transform.DOLocalMoveY(UnityEngine.Random.Range(-1f, 1f) * 20f, randomDurationMoveY).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetRelative(true);

            var randomDurationMoveX = UnityEngine.Random.Range(0.4f, 1f);
            transform.DOLocalMoveX(UnityEngine.Random.Range(-1f, 1f) * 20f, randomDurationMoveX).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
        }

        public async UniTask Move()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayMove));
            transform.DOKill();
            var distance = Vector3.Distance(transform.position, _target.position);
            var path = GetQuadraticBezierPoints(transform.position, _target.position, _curveHeigh * distance);
            await transform.DOPath(path, 0.8f, PathType.Linear, PathMode.Sidescroller2D).SetEase(Ease.InSine);
            gameObject.SetActive(false);
        }

        static Vector3[] GetQuadraticBezierPoints(Vector3 startpoint, Vector3 endPoint, float curveHeigh)
        {
            var dir = endPoint - startpoint;
            var dir2 = dir.ToVector2().Rotate(90);
            var centerPoint = Vector3.Lerp(startpoint, endPoint, 0.2f);
            Vector3 heighPoint = centerPoint + dir2.ToVector3().normalized * curveHeigh;

            Vector3[] res = new Vector3[6];
            float maxT = 1f;
            int index = 0;

            for (float t = 0; t <= maxT; t += 0.2f)
            {
                Vector3 newPoint = (Mathf.Pow(1 - t, 2) * startpoint) + (2 * (1 - t) * t * heighPoint) +
                                   (t * t * endPoint);
                try
                {
                    res[index++] = newPoint;
                }
                catch
                {
                    break;
                }
            }

            return res;
        }
    }
}