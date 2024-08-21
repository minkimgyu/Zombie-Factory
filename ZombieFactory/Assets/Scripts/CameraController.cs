using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _cameraParent;
    [SerializeField] Transform _mainCamera;

    public void MoveCamera(Vector3 cameraHolderPosition, Vector3 viewRotation)
    {
        transform.position = cameraHolderPosition;
        _mainCamera.rotation = Quaternion.Euler(viewRotation.x, viewRotation.y, 0);
    }
}