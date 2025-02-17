using UnityEngine.Events;

namespace Kelsey
{
    public interface IIapService
    {
        void RegisterOnIapSuccess(UnityAction<string, float> onPurchaseIap);
        void BuyProduct(string sku, UnityAction onPurchaseCompleted, UnityAction<string> onFail = null);
        string GetPriceString(string getSku);
        void RestorePurchases();
    }
}