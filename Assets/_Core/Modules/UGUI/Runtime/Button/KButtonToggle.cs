using UnityEngine;

namespace Kelsey.UGUI
{
    public abstract class KButtonToggle : KButton
    {
        [SerializeField] protected GameObject activeGroup, deactiveGroup;

        void OnEnable()
        {
            UpdateDisplay();
        }

        protected override void OnClick()
        {
            base.OnClick();
            InverseState();
            UpdateDisplay();
        }


        void UpdateDisplay()
        {
            activeGroup.SetActive(GetCurrentState());
            deactiveGroup.SetActive(!GetCurrentState());
        }

        protected abstract void InverseState();

        protected abstract bool GetCurrentState();
    }
}