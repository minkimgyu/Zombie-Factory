using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour, ISoundControllable
{
    Dictionary<ISoundControllable.SoundName, AudioClip> _clipDictionary;

    BaseFactory _soundFactory;

    AudioSource _bgmPlayer;
    AudioSource[] _sfxPlayer;

    [SerializeField] GameObject _bgmPlayerObject;
    [SerializeField] GameObject _sfxPlayerObject;

    public void Initialize(Dictionary<ISoundControllable.SoundName, AudioClip> clipDictionary, BaseFactory soundFactory)
    {
        _clipDictionary = clipDictionary;
        _soundFactory = soundFactory;

        _bgmPlayer = _bgmPlayerObject.GetComponent<AudioSource>();
        _sfxPlayer = _bgmPlayerObject.GetComponents<AudioSource>();
    }

    public void PlayBGM(ISoundControllable.SoundName name)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        _bgmPlayer.clip = _clipDictionary[name];
        _bgmPlayer.Play();
    }

    public void PlaySFX(ISoundControllable.SoundName name, Vector3 pos)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        SoundPlayer soundPlayer = _soundFactory.Create();
        soundPlayer.transform.position = pos;
        soundPlayer.Play(_clipDictionary[name]);
    }

    public void PlaySFX(ISoundControllable.SoundName name)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

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