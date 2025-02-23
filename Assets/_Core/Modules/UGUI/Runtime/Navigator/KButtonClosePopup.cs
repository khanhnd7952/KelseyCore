namespace Kelsey.UGUI
{
    public class KButtonClosePopup : KButton
    {
        private PopupBase _popup;

        protected override void OnAwake()
        {
            base.OnAwake();
            _popup = GetComponentInParent<PopupBase>();
        }

        protected override void OnClick()
        {
            base.OnClick();
            _popup.Exit();
        }
    }
}