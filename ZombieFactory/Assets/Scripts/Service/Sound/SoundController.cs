using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour, ISoundControllable
{
    Dictionary<ISoundControllable.SoundName, AudioClip> _clipDictionary;

    BaseFactory _soundFactory;
    AudioSource _bgmPlayer;
    AudioSource[] _sfxPlayers;

    [SerializeField] GameObject _bgmPlayerObject;
    [SerializeField] GameObject _sfxPlayerObject;

    public void Initialize(Dictionary<ISoundControllable.SoundName, AudioClip> clipDictionary, BaseFactory soundFactory)
    {
        _clipDictionary = clipDictionary;
        _soundFactory = soundFactory;

        _bgmPlayer = _bgmPlayerObject.GetComponent<AudioSource>();
        _bgmPlayer.loop = true;

        _sfxPlayers = _sfxPlayerObject.GetComponents<AudioSource>();
    }

    public void PlayBGM(ISoundControllable.SoundName name)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        _bgmPlayer.clip = _clipDictionary[name];
        _bgmPlayer.Play();
    }

    public void PlayBGM(ISoundControllable.SoundName name, float volume)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        _bgmPlayer.clip = _clipDictionary[name];
        _bgmPlayer.volume = volume;
        _bgmPlayer.Play();
    }

    public void PlaySFX(ISoundControllable.SoundName name, Vector3 pos, float volume)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        SoundPlayer soundPlayer = _soundFactory.Create();
        soundPlayer.transform.position = pos;
        soundPlayer.ResetVolume(volume);
        soundPlayer.Play(_clipDictionary[name]);
    }

    public void PlaySFX(ISoundControllable.SoundName name)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            if (_sfxPlayers[i].isPlaying == true) continue;

            _sfxPlayers[i].clip = _clipDictionary[name];
            _sfxPlayers[i].Play();
        }
    }

    public void PlaySFX(ISoundControllable.SoundName name, float volume)
    {
        if (_clipDictionary.ContainsKey(name) == false) return;

        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            if (_sfxPlayers[i].isPlaying == true) continue;

            _sfxPlayers[i].clip = _clipDictionary[name];
            _sfxPlayers[i].volume = volume;
            _sfxPlayers[i].Play();
        }
    }

    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    public void StopAllSound()
    {
        _bgmPlayer.Stop();
        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            _sfxPlayers[i].Stop();
        }
    }
}