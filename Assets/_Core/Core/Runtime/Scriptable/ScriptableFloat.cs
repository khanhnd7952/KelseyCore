using UnityEngine;

namespace Kelsey
{
    [CreateAssetMenu(menuName = BASE_PATH + FILE_NAME, fileName = FILE_NAME)]
    public class ScriptableFloat : ScriptableVariable<float>
    {
        const string FILE_NAME = "scriptable_float";

        protected override void SavePrefValue() => PlayerPrefs.SetFloat(prefKey, Value);
        protected override void LoadPrefValue() => SetValue(PlayerPrefs.GetFloat(prefKey, defaultValue));
        public override void ParseValue(string data) => SetValue(float.Parse(data));
        public override string GetStringValue() => Value.ToString();
    }
}