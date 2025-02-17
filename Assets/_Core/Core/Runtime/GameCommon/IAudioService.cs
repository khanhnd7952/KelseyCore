using UnityEngine;

namespace Kelsey
{
    public interface IAudioService
    {
        void PlaySoundUI(AudioClip clip);
        void PlaySoundFx(AudioClip clip);


        void PlaySoundClick();
        void PlaySoundCoin();
    }
}