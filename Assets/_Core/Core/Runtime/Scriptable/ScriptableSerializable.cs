using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Kelsey
{
    public class ScriptableSerializable<TValue> : ScriptableVariable<TValue>
    {
        public override void ParseValue(string data)
        {
            try
            {
                SetValue(JsonConvert.DeserializeObject<TValue>(data));
            }
            catch (Exception e)
            {
                SetValue(defaultValue);
            }
        }

        public override string GetStringValue() => JsonConvert.SerializeObject(Value);

        protected override void SavePrefValue() => PlayerPrefs.SetString(prefKey, JsonConvert.SerializeObject(Value));

        protected override void LoadPrefValue()
        {
            ParseValue(PlayerPrefs.GetString(prefKey));
        }
    }
}