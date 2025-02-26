namespace Kelsey
{
    public interface IAdsIntervalService
    {
        public bool CanShowAds();
        public float GetAdsInterval();
        public int GetLevelShowAds();
    }
}