using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundControllable
{
    public enum SoundName
    {
        Fire,
        Reload,
        Die
    }

    void PlayBGM(SoundName name);
    void PlaySFX(SoundName name);
    void PlaySFX(SoundName name, Vector3 pos);

    void StopBGM();
    void StopAllSound();
}