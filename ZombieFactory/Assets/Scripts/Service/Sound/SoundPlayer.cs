using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour, ISoundPlayable
{
    Dictionary<ISoundPlayable.SoundName, AudioClip> _clipDictionary;

    AudioSource _bgmPlayer;
    AudioSource[] _sfxPlayer;

    [SerializeField] GameObject _bgmPlayerObject;
    [SerializeField] GameObject _sfxPlayerObject;

    public void Initialize(Dictionary<ISoundPlayable.SoundName, AudioClip> clipDictionary)
    {
        _clipDictionary = clipDictionary;
        _bgmPlayer = _bgmPlayerObject.GetComponent<AudioSource>();
        _sfxPlayer = _bgmPlayerObject.GetComponents<AudioSource>();
    }

    public void PlayBGM(ISoundPlayable.SoundName name)
    {
        _bgmPlayer.clip = _clipDictionary[name];
        _bgmPlayer.Play();
    }

    public void PlaySFX(ISoundPlayable.SoundName name)
    {
        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            if (_sfxPlayer[i].isPlaying == true) continue;

            _sfxPlayer[i].clip = _clipDictionary[name];
            _sfxPlayer[i].Play();
        }
    }

    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    public void StopAllSound()
    {
        _bgmPlayer.Stop();
        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            _sfxPlayer[i].Stop();
        }
    }
}