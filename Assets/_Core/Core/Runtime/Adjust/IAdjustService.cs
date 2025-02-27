using UnityEngine.Events;

#if KELSEY_UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace Kelsey
{
    public interface IAdjustService
    {
        bool IsAdjustInitialized { get; }

        void TrackAdsRevenue(
            double value,
            string network,
            string unit,
            string placement
        );

#if KELSEY_UNITY_PURCHASING
        void TrackIapRevenue(PurchaseEventArgs purchaseEventArgs);
#endif

        void RegisterOnGetCampaign(UnityAction<EAdjustCampaign> action);
        EAdjustCampaign GetCampaign();
    }
}