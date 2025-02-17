namespace Kelsey
{
    public class PrefsStringArray : KPrefs<string[]>
    {
        public PrefsStringArray(string key, string[] defaultValue) : base(key, defaultValue)
        {
        }
    }
}