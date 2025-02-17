namespace Kelsey
{
    public interface IAdsTracking
    {
        void TrackEventPaidAdsImpression(
            string adPlatform,
            string currency,
            double value,
            string adUnitName,
            string adNetwork,
            string adFormat,
            string placement);

        void TrackEventAdsImpression(
            string adPlatform,
            string currency,
            double value,
            string adUnitName,
            string adNetwork,
            string adFormat,
            string placement);
    }
}