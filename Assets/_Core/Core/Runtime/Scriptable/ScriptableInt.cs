using UnityEngine;

namespace Kelsey
{
    [CreateAssetMenu(menuName = BASE_PATH + FILE_NAME, fileName = FILE_NAME)]
    public class ScriptableInt : ScriptableVariable<int>
    {
        const string FILE_NAME = "scriptable_int";
        
        protected override void SavePrefValue() => PlayerPrefs.SetInt(prefKey, Value);
        protected override void LoadPrefValue() => SetValue(PlayerPrefs.GetInt(prefKey, defaultValue));
        public override void ParseValue(string data) => SetValue(int.Parse(data));
        public override string GetStringValue() => Value.ToString();
    }
}