using System;
using Kelsey.Extension;
using TMPro;
using UnityEngine;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class TextTimer : UpdateOptimizeMonoBehaviour
    {
        TextMeshProUGUI txtTimer;
        string _txt;
        TimeSpan _time;

        protected virtual void Awake()
        {
            txtTimer = GetComponent<TextMeshProUGUI>();
        }

        protected override void DoUpdate()
        {
            _txt = "";
            _time = GetTime();
            if (_time.Days > 0) _txt += _time.Days + "d ";
            _txt += _time.Hours.ConvertTo00Number() + "h ";
            _txt += _time.Minutes.ConvertTo00Number() + "m";
            txtTimer.SetText(_txt);
        }

        protected abstract TimeSpan GetTime();
    }
}