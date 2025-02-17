using System.Collections;
using TMPro;
using UnityEngine;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimateText : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] string prefix;
        [SerializeField] string textToAnimate;

        void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        void OnEnable()
        {
            if (_coroutineAnimateText != null) StopCoroutine(_coroutineAnimateText);
            _coroutineAnimateText = StartCoroutine(corouAnimateText());
        }

        void OnDisable()
        {
            if (_coroutineAnimateText != null) StopCoroutine(_coroutineAnimateText);
        }


        Coroutine _coroutineAnimateText;

        private IEnumerator corouAnimateText()
        {
            _textMeshProUGUI.text = prefix;
            while (true)
            {
                for (var i = 0; i < textToAnimate.Length; i++)
                {
                    _textMeshProUGUI.text = prefix + textToAnimate.Substring(0, i);
                    yield return new WaitForSecondsRealtime(0.2f);
                }
            }
            yield break;
        }
    }
}