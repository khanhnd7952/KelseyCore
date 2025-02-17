using System;

namespace Kelsey
{
    public interface IAdsService
    {
        bool IsRewardedReady();
        void ShowRewarded(string position, Action onClaim, Action onFail);
        bool IsInterstitialReady();
        void ShowInterstitial(string position, Action onDone);
        void DestroyBanner();
        float GetBannerHeight();
        void CheckShowBanner();
        void RegisterOnInterstitialSuccess(Action onShowInterstitial);
        void RegisterOnRewardedSuccess(Action onShowRewarded);
    }
}