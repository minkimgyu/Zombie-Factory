using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocater
{
    static ISoundPlayable _soundPlayer;
    static NullSoundPlayer _nullSoundPlayer;

    static IInputable _inputController;
    static NullInputHandler _nullInputController;

    public static void Initialize()
    {
        _nullSoundPlayer = new NullSoundPlayer();
        _nullInputController = new NullInputHandler();
    }

    public static void Provide(ISoundPlayable soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }

    public static void Provide(IInputable inputController)
    {
        _inputController = inputController;
    }

    public static ISoundPlayable ReturnSoundPlayer()
    {
        if (_soundPlayer == null) return _nullSoundPlayer;
        return _soundPlayer;
    }

    public static IInputable ReturnInputHandler()
    {
        if (_inputController == null) return _nullInputController;
        return _inputController;
    }
}