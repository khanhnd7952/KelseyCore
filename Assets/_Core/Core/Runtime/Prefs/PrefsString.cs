namespace Kelsey
{
    public class PrefsString : KPrefs<string>
    {
        public PrefsString(string key, string defaultValue) : base(key, defaultValue)
        {
        }
    }
}