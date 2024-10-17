using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullSoundControllable : ISoundControllable
{
    public void PlayBGM(ISoundControllable.SoundName name) { }

    public void PlaySFX(ISoundControllable.SoundName name) { }
    public void PlaySFX(ISoundControllable.SoundName name, Vector3 pos) { }

    public void StopAllSound() { }
    public void StopBGM() { }
}