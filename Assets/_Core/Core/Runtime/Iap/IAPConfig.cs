using UnityEngine;

namespace Kelsey
{
    [CreateAssetMenu(fileName = "IAPConfig", menuName = "IAP/IAPConfig", order = 0)]
    public class IAPConfig : ScriptableObject
    {
        public string skuAndroid;
        public string skuIOS;
        public string packName;
        public int priceInDollar;
        public string defaultPriceStringInDollar;

        public string GetSku()
        {
            var sku = "";
#if UNITY_ANDROID || UNITY_EDITOR
            sku = skuAndroid;
#elif UNITY_IOS
            sku = skuIOS;
#endif
            return sku;
        }
    }
}