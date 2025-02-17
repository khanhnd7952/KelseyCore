using System;

namespace Kelsey
{
    public interface IVibrationPersistentData
    {
        bool Vibration { get; set; }
        void RegisterOnVibrationChange(Action<bool> action);
        void UnregisterOnVibrationChange(Action<bool> action);
    }
}