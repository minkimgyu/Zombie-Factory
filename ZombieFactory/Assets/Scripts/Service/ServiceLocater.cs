using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocater
{
    static ISoundControllable _soundPlayer;
    static NullSoundControllable _nullSoundPlayer;

    static IInputable _inputController;
    static NullInputHandler _nullInputController;

    static ISceneControllable _sceneController;
    static NullSceneController _nullSceneController;

    static ITimeControllable _timeControllable;
    static NullTimeControllable _nullTimeControllable;

    public static void Initialize()
    {
        _nullSoundPlayer = new NullSoundControllable();
        _nullInputController = new NullInputHandler();
        _nullSceneController = new NullSceneController();
        _nullTimeControllable = new NullTimeControllable();
    }

    public static void Provide(ISoundControllable soundPlayer)
    {
        _soundPlayer = soundPlayer;
    }

    public static void Provide(ISceneControllable sceneController)
    {
        _sceneController = sceneController;
    }

    public static void Provide(IInputable inputController)
    {
        _inputController = inputController;
    }

    public static void Provide(ITimeControllable timeControllable)
    {
        _timeControllable = timeControllable;
    }


    public static ISceneControllable ReturnSceneController()
    {
        if (_sceneController == null) return _nullSceneController;
        return _sceneController;
    }

    public static ISoundControllable ReturnSoundPlayer()
    {
        if (_soundPlayer == null) return _nullSoundPlayer;
        return _soundPlayer;
    }

    public static IInputable ReturnInputHandler()
    {
        if (_inputController == null) return _nullInputController;
        return _inputController;
    }

    public static ITimeControllable ReturnTimeController()
    {
        if (_timeControllable == null) return _nullTimeControllable;
        return _timeControllable;
    }
}