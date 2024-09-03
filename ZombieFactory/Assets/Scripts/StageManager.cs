using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] InputHandler _inputHandler;

    private void Awake()
    {
        ServiceLocater.Provide(_inputHandler);
    }
}
