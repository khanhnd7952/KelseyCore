using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kelsey
{
    public class PlayerPrefsValueAdditionSuffixes<T>
    {
        private string key;
        private T defaultValue;

        public PlayerPrefsValueAdditionSuffixes(string key, T defaultValue)
        {
            this.key = key;
            this.defaultValue = defaultValue;
        }

        string GetPrefKey(string suffixes)
        {
            return key + "_" + suffixes;
        }

        public T GetValue(string suffixes)
        {
            if (PlayerPrefs.HasKey(GetPrefKey(suffixes)))
            {
                string serializedValue = PlayerPrefs.GetString(GetPrefKey(suffixes));
                return JsonConvert.DeserializeObject<T>(serializedValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetValue(string suffixes, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(GetPrefKey(suffixes), serializedValue);
            onValueChange?.Invoke();
        }

        public Action onValueChange;
    }


    public class KPrefsSuffixes<TSuffixesType, TValue>
    {
        private string key;
        private TValue defaultValue;

        public KPrefsSuffixes(string key, TValue defaultValue)
        {
            this.key = key;
            this.defaultValue = defaultValue;
        }

        string GetPrefKey(TSuffixesType suffixes)
        {
            return key + "_" + suffixes.ToString();
        }

        public TValue GetValue(TSuffixesType suffixes)
        {
            if (PlayerPrefs.HasKey(GetPrefKey(suffixes)))
            {
                string serializedValue = PlayerPrefs.GetString(GetPrefKey(suffixes));
                return JsonConvert.DeserializeObject<TValue>(serializedValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetValue(TSuffixesType suffixes, TValue value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(GetPrefKey(suffixes), serializedValue);
            OnValueChange?.Invoke(suffixes, value);
        }

        public Action<TSuffixesType, TValue> OnValueChange { get; set; }
    }
}