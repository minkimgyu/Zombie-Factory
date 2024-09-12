using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, IPoint
{
    Camera[] _cameras;

    public void Initialize()
    {
        _cameras = GetComponentsInChildren<Camera>();

        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.Type.MoveCamera, new MoveCameraCommand(MoveCamera));
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.Type.ChangeFieldOfView, new ChangeFieldOfViewCommand(OnFieldOfViewChangeRequested));
    }

    public void MoveCamera(Vector3 cameraHolderPosition, Vector3 viewRotation)
    {
        transform.position = cameraHolderPosition;
        transform.rotation = Quaternion.Euler(viewRotation.x, viewRotation.y, 0);
    }

    public void OnFieldOfViewChangeRequested(float fieldOfView, float ratio)
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            _cameras[i].fieldOfView = Mathf.Lerp(_cameras[i].fieldOfView, fieldOfView, ratio);
        }
    }

    public Vector3 ReturnDirection()
    {
        return transform.forward;
    }

    public Vector3 ReturnPosition()
    {
        return transform.position;
    }
}