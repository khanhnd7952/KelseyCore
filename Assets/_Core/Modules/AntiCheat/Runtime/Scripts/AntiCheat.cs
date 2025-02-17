using System;
using Sisus.Init;
#if UNITY_ANDROID && !UNITY_EDITOR
using System;
#endif


namespace Kelsey.AntiCheat
{
    [Service(typeof(IAntiCheatService))]
    [Serializable]
    public class AntiCheat : IAntiCheatService
    {
        bool? _isCheater;

        PrefsBool _isBypass { get; } = new PrefsBool("anticheat_is_bypass", false);

        public bool IsCheater
        {
            get
            {
                if (_isCheater == null) CheckCheater();
                return _isCheater ?? false;
            }
        }

        public bool IsBypassCheat
        {
            get => _isBypass.Value;
            set => _isBypass.Value = value;
        }

        void CheckCheater()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                _isCheater = !AppInstallationSourceValidator.IsInstalledFromGooglePlay();
                return;
            }
            catch (Exception e)
            {
                // ignored
            }
#endif
            _isCheater = false;
        }
    }
}