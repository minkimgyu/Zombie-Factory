using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundPlayable
{
    public enum SoundName
    {

        Die
    }

    //void ResetData(Dictionary<SoundName, AudioClip> clipDictionary);
    void PlayBGM(SoundName name);
    void PlaySFX(SoundName name);

    void StopBGM();
    void StopAllSound();
}