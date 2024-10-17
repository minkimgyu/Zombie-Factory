using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour, IInputable
{
    Dictionary<IInputable.Type, BaseCommand> _inputEvents;

    public InputHandler()
    {
        _inputEvents = new Dictionary<IInputable.Type, BaseCommand>();
    }

    private void Update()
    {
        float viewX = Input.GetAxisRaw("Mouse X");
        float viewY = Input.GetAxisRaw("Mouse Y");
        Vector2 viewDirection = new Vector2(viewX, viewY);
        Execute(IInputable.Type.View, viewDirection);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;
        Execute(IInputable.Type.Move, direction);

        if(Input.GetMouseButtonDown(0))
        {
            Execute(IInputable.Type.EventStart, BaseWeapon.EventType.Main);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Execute(IInputable.Type.EventEnd, BaseWeapon.EventType.Main);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Execute(IInputable.Type.EventStart, BaseWeapon.EventType.Sub);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Execute(IInputable.Type.EventEnd, BaseWeapon.EventType.Sub);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Execute(IInputable.Type.Equip, BaseWeapon.Type.Main);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Execute(IInputable.Type.Equip, BaseWeapon.Type.Sub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Execute(IInputable.Type.Equip, BaseWeapon.Type.Melee);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Execute(IInputable.Type.Reload);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Execute(IInputable.Type.Interact);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Execute(IInputable.Type.Drop);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Execute(IInputable.Type.Escape);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Execute(IInputable.Type.Jump);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Execute(IInputable.Type.CrouchStart);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Execute(IInputable.Type.CrouchEnd);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Execute(IInputable.Type.RunStart);
        }
        else
        {
            Execute(IInputable.Type.RunEnd);
        }
    }

    void Execute(IInputable.Type type)
    {
        if(_inputEvents.ContainsKey(type) == false) return;
        _inputEvents[type].Execute();
    }

    void Execute(IInputable.Type type, Vector2 direction)
    {
        if (_inputEvents.ContainsKey(type) == false) return;

        _inputEvents[type].Execute(direction);
    }

    void Execute(IInputable.Type type, Vector3 direction)
    {
        if (_inputEvents.ContainsKey(type) == false) return;

        _inputEvents[type].Execute(direction);
    }

    void Execute(IInputable.Type type, BaseWeapon.Type weaponType)
    {
        if (_inputEvents.ContainsKey(type) == false) return;

        _inputEvents[type].Execute(weaponType);
    }

    void Execute(IInputable.Type type, BaseWeapon.EventType eventType)
    {
        if (_inputEvents.ContainsKey(type) == false) return;

        _inputEvents[type].Execute(eventType);
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
