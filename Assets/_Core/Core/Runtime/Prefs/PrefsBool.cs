namespace Kelsey
{
    public class PrefsBool : KPrefs<bool>
    {
        public PrefsBool(string key, bool defaultValue) : base(key, defaultValue)
        {
        }
    }
}