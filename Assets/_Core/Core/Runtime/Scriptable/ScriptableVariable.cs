using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Kelsey
{
    public abstract class ScriptableVariable<TValue> : ScriptableBase
    {
        protected const string BASE_PATH = "Scriptable/";

        [SerializeField] protected TValue defaultValue;
        [SerializeField] private bool saved;
        [SerializeField, ShowIf("saved")] protected string prefKey;
        TValue _value;

        [ShowInInspector, LabelText("Runtime Value")]
        public TValue Value => _value;

        public void SetValue(TValue value)
        {
            _value = value;
            if (saved) SavePrefValue();
            _onValueChanged?.Invoke(value);
        }

        protected abstract void SavePrefValue();
        protected abstract void LoadPrefValue();
        void LoadDefaultValue() => _value = defaultValue;

        protected virtual void OnEnable()
        {
            if (saved && PlayerPrefs.HasKey(prefKey)) LoadPrefValue();
            else LoadDefaultValue();
        }

        protected void OnDisable() => _value = default;

        public void RegisterOnValueChanged(UnityAction<TValue> action) => _onValueChanged += action;
        public void UnregisterOnValueChanged(UnityAction<TValue> action) => _onValueChanged -= action;

        UnityAction<TValue> _onValueChanged;

#if UNITY_EDITOR
        private void Awake()
        {
            SetPrefKeyToGuid();
        }

        void SetPrefKeyToGuid()
        {
            var guid = Guid.NewGuid().ToString();
            prefKey = guid;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}