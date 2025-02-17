using UnityEngine;

namespace Kelsey.UGUI
{
    public class KButtonOpenUrl : KButton
    {
        [SerializeField] private string androidUrl;
        [SerializeField] private string iosUrl;

        protected override void OnClick()
        {
            base.OnClick();

#if UNITY_ANDROID
            Application.OpenURL(androidUrl);
#elif UNITY_IOS
            Application.OpenURL(iosUrl);
#endif
        }
    }
}