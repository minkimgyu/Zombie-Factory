using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour, IInputable
{
    Dictionary<IInputable.Type, BaseCommand> _inputEvents = new Dictionary<IInputable.Type, BaseCommand>();

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        _inputEvents[IInputable.Type.Move].Execute(new Vector3(x, 0, z).normalized);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _inputEvents[IInputable.Type.Escape]?.Execute();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _inputEvents[IInputable.Type.Jump]?.Execute();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _inputEvents[IInputable.Type.CrouchStart]?.Execute();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _inputEvents[IInputable.Type.CrouchEnd]?.Execute();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _inputEvents[IInputable.Type.RunStart]?.Execute();
        }
        else
        {
            _inputEvents[IInputable.Type.RunEnd]?.Execute();
        }
    }

    public void AddEvent(IInputable.Type type, BaseCommand command)
    {
        _inputEvents.Add(type, command);
    }

    public void RemoveEvent(IInputable.Type type)
    {
        _inputEvents.Remove(type);
    }
}
