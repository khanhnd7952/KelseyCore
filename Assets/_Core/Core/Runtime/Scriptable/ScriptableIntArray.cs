using System;
using UnityEngine;

namespace Kelsey
{
    [CreateAssetMenu(menuName = BASE_PATH + FILE_NAME, fileName = FILE_NAME)]
    public class ScriptableIntArray : ScriptableVariable<int[]>
    {
        private const string FILE_NAME = "scriptable_int_array";

        public override void ParseValue(string data)
        {
            try
            {
                SetValue(Array.ConvertAll(data.Split(','), int.Parse));
            }
            catch (Exception e)
            {
                SetValue(defaultValue);
            }
        }

        public override string GetStringValue() => string.Join(",", Value);

        protected override void SavePrefValue() => PlayerPrefs.SetString(prefKey, string.Join(",", Value));

        protected override void LoadPrefValue() =>
            ParseValue(PlayerPrefs.GetString(prefKey, string.Join(",", defaultValue)));
    }
}