using System.Collections;
using UnityEngine;

namespace Kelsey.UGUI
{
    public class MoveToCoin : MonoBehaviour
    {
        public AnimationCurve animationCurve;
        public Transform destination;

        private Coroutine coroutine;
        private Vector3 start;
        private float duration;

        private void OnEnable()
        {
            start = transform.position;
            duration = Random.Range(0.6f, 0.9f);
            transform.localScale = Vector3.one * Random.Range(0.7f, 1.2f);
            if (coroutine == null) { coroutine = StartCoroutine(Move()); }
            animationCurve.AddKey(new Keyframe(0.5f, Random.Range(-1f, 1f)));
        }

        IEnumerator Move()
        {
            float time = 0f;

            Vector2 end = destination.position;

            while (time < duration)
            {

                time += Time.deltaTime;
                float normalizedTimeOnCurve = time / duration;
                float yValueOfCurve = animationCurve.Evaluate(normalizedTimeOnCurve);

                transform.position = Vector2.Lerp(start, end, normalizedTimeOnCurve) + new Vector2(yValueOfCurve, 0f);
                yield return null;
            }

            coroutine = null;
            gameObject.SetActive(false);

        }

        private void OnDisable()
        {
            transform.position = Vector2.zero;
            animationCurve.RemoveKey(1);
        }
    }
}