using System;

namespace Kelsey
{
    public interface IAudioPersistentData
    {
        public bool Music { get; set; }
        public bool Sound { get; set; }

        public void RegisterOnSoundChange(Action<bool> action);
        public void UnregisterOnSoundChange(Action<bool> action);
        public void RegisterOnMusicChange(Action<bool> action);
        public void UnregisterOnMusicChange(Action<bool> action);
    }
}