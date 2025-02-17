using System;

namespace Kelsey
{
    public interface IRemoveAdsService
    {
        bool IsRemoveAds();
        void RemoveAds();
        void RegisterRemoveAds(Action<bool> listener);
        void UnRegisterRemoveAds(Action<bool> listener);
    }
}