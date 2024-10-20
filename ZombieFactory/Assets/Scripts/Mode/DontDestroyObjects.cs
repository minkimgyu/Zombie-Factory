using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjects : MonoBehaviour
{
    public void Initialize(
        AddressableHandler addressableHandler,
        InputHandler inputHandler,
        SoundController soundController) 
    {
        _addressableHandler = addressableHandler;
        _inputHandler = inputHandler;
        _soundController = soundController;

        _soundController.transform.SetParent(transform);
        DontDestroyOnLoad(gameObject);
    }

    AddressableHandler _addressableHandler;
    public AddressableHandler AddressableHandler 
    {
        get { return _addressableHandler; } 
    }

    InputHandler _inputHandler;
    public InputHandler InputHandler
    {
        get { return _inputHandler; }
    }

    SoundController _soundController;
    public SoundController SoundController
    {
        get { return _soundController; }
    }

    public void Update()
    {
        _inputHandler.OnUpdate();
    }
}
