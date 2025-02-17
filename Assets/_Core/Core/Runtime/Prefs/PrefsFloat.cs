namespace Kelsey
{
    public class PrefsFloat : KPrefs<float>
    {
        public PrefsFloat(string key, float defaultValue) : base(key, defaultValue)
        {
        }
    }
}