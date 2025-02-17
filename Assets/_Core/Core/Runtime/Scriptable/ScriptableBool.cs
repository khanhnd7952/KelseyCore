using UnityEngine;

namespace Kelsey
{
    [CreateAssetMenu(menuName = BASE_PATH + FILE_NAME, fileName = FILE_NAME)]
    public class ScriptableBool : ScriptableVariable<bool>
    {
        const string FILE_NAME = "scriptable_bool";
        protected override void SavePrefValue() => PlayerPrefs.SetInt(prefKey, Value ? 1 : 0);
        protected override void LoadPrefValue() => SetValue(PlayerPrefs.GetInt(prefKey, defaultValue ? 1 : 0) == 1);
        public override void ParseValue(string data) => SetValue(bool.Parse(data));
        public override string GetStringValue() => Value.ToString();
    }
}