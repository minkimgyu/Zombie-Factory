using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveWeaponViewerCommand : BaseCommand
{
    Action<bool, string, Vector3> ActiveWeaponViewerEvent;

    public ActiveWeaponViewerCommand(Action<bool, string, Vector3> ActiveWeaponViewerEvent)
    {
        this.ActiveWeaponViewerEvent = ActiveWeaponViewerEvent;
    }

    public override void Execute(bool nowActivate, string name, Vector3 position)
    {
        ActiveWeaponViewerEvent?.Invoke(nowActivate, name, position);
    }
}

public class SpawnEffectCommand : BaseCommand
{
    Action<BaseEffect.Name, Vector3, Vector3> SpawnEffect;

    public SpawnEffectCommand(Action<BaseEffect.Name, Vector3, Vector3> SpawnEffect)
    {
        this.SpawnEffect = SpawnEffect;
    }
    public override void Execute(BaseEffect.Name name, Vector3 hitPosition, Vector3 hitNormal)
    {
        SpawnEffect?.Invoke(name, hitPosition, hitNormal);
    }
}

public class ActivateCommand : BaseCommand
{
    Action<bool> ActivateEvent;

    public ActivateCommand(Action<bool> SpawnEffect)
    {
        this.ActivateEvent = SpawnEffect;
    }
    public override void Execute(bool activate)
    {
        ActivateEvent?.Invoke(activate);
    }
}

public class ObserverEventBus : BaseBus<ObserverEventBus.Type>
{
    public enum Type
    {
        ActiveZoom,
        MoveCamera,
        ChangeFieldOfView,

        ActiveCrosshair,
        ActiveAmmoViewer,
        ChageAmmoCount,

        AddPreview,
        RemovePreview,

        SpawnEffect,

        ActiveInteractableInfo,
    }

    public override void Publish(Type type, BaseEffect.Name name, Vector3 hitPosition, Vector3 hitNormal) 
    {
        if (_commands.ContainsKey(type) == false) return;  
        for (int i = 0; i < _commands[type].Count; i++)
        {
            _commands[type][i].Execute(name, hitPosition, hitNormal);
        }
    }

    public override void Publish(Type type, bool nowActivate, string name, Vector3 position)
    {
        if (_commands.ContainsKey(type) == false) return;

        for (int i = 0; i < _commands[type].Count; i++)
        {
            _commands[type][i].Execute(nowActivate, name, position);
        }
    }

    public override void Publish(Type type, bool nowActivate)
    {
        if (_commands.ContainsKey(type) == false) return;

        for (int i = 0; i < _commands[type].Count; i++)
        {
            _commands[type][i].Execute(nowActivate);
        }
    }
}
