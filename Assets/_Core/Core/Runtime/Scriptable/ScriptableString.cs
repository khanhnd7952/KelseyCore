using UnityEngine;

namespace Kelsey
{
    [CreateAssetMenu(menuName = BASE_PATH + FILE_NAME, fileName = FILE_NAME)]
    public class ScriptableString : ScriptableVariable<string>
    {
        const string FILE_NAME = "scriptable_string";
        protected override void SavePrefValue() => PlayerPrefs.SetString(prefKey, Value);
        protected override void LoadPrefValue() => SetValue(PlayerPrefs.GetString(prefKey, defaultValue));
        public override void ParseValue(string data) => SetValue(data);
        public override string GetStringValue() => Value;
    }
}