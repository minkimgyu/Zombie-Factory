using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : PoolObject
{
    AudioSource _audioSource;

    public override void Initialize()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ResetVolume(float ratio)
    {
        _audioSource.volume = ratio;
    }

    public void Play(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();

        StartTimer(clip.length);
    }
}
