namespace Kelsey
{
    public class PrefsInt : KPrefs<int>
    {
        public PrefsInt(string key, int defaultValue) : base(key, defaultValue)
        {
        }
    }
}