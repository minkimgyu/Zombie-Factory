using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveCameraCommand : BaseCommand
{
    Action<Vector3, Vector3> MoveEvent;

    public MoveCameraCommand(Action<Vector3, Vector3> MoveEvent)
    {
        this.MoveEvent = MoveEvent;
    }

    public override void Execute(Vector3 cameraHolderPosition, Vector3 viewRotation)
    {
        MoveEvent?.Invoke(cameraHolderPosition, viewRotation);
    }
}

public class ChangeFieldOfViewCommand : BaseCommand
{
    Action<float, float> ChangeEvent;

    public ChangeFieldOfViewCommand(Action<float, float> ChangeEvent)
    {
        this.ChangeEvent = ChangeEvent;
    }

    public override void Execute(float fieldOfView, float ratio)
    {
        ChangeEvent?.Invoke(fieldOfView, ratio);
    }
}

public class ChangeAmmoCommand : BaseCommand
{
    Action<int, int> ChangeEvent;

    public ChangeAmmoCommand(Action<int, int> ChangeEvent)
    {
        this.ChangeEvent = ChangeEvent;
    }

    public override void Execute(int inMagazine, int inPossession)
    {
        ChangeEvent?.Invoke(inMagazine, inPossession);
    }
}

public class ActiveCommand : BaseCommand
{
    Action<bool> ActivateEvent;

    public ActiveCommand(Action<bool> ActivateEvent)
    {
        this.ActivateEvent = ActivateEvent;
    }

    public override void Execute(bool active)
    {
        ActivateEvent?.Invoke(active);
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
    }
}
