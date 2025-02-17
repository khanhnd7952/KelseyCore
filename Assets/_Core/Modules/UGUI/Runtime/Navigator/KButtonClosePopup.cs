namespace Kelsey.UGUI
{
    public class KButtonClosePopup : KButton
    {
        private Popup _popup;

        private void Awake()
        {
            _popup = GetComponentInParent<Popup>();
        }

        protected override void OnClick()
        {
            base.OnClick();
            _popup.Exit();
        }
    }
}