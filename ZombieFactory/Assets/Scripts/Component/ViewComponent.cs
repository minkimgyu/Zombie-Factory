using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ViewComponent : MonoBehaviour //, IRecoilReceiver
{
    [SerializeField] private Transform _direction;
    [SerializeField] private Transform _cameraHolder;

    [SerializeField] private float _viewYRange = 40;
    [SerializeField] private Vector2 _viewSensitivity;

    Vector3 _viewRotation;

    float _y, _x = 0;

    Action<Vector3, Vector3> MoveCamera; // LateUpdate에서 돌리기
    Rigidbody _rigid;

    public void Initialize()
    {
        _rigid = GetComponent<Rigidbody>();
        CameraController controller = FindObjectOfType<CameraController>();
        MoveCamera = controller.MoveCamera;

        //_viewYRange = viewYRange;
        //_viewSensitivity = viewSensitivity;
    }

    Quaternion rotation1;

    // 이 부분은 플레이어 컨트롤러에서 돌려주자
    public void ResetView()
    {
        Vector3 newViewRotation = Vector3.zero;
        newViewRotation.y = _viewRotation.y + Input.GetAxisRaw("Mouse X") * _viewSensitivity.y;


        _viewRotation = Vector3.Lerp(_viewRotation, newViewRotation, Time.deltaTime);


        //_viewRotation.y = Mathf.Clamp(_viewRotation.y - (Input.GetAxisRaw("Mouse Y") * _viewSensitivity.y * Time.deltaTime), -_viewYRange, _viewYRange);

        //_direction.rotation = Quaternion.Euler(0, _viewRotation.x, 0);
        //_actorBone.rotation = Quaternion.Euler(0, _viewRotation.x + 45, 0); // ActorBoneViewRotation.y, _direction.eulerAngles.y, 0
    }

    public void Rotation()
    {
        Quaternion deltaRotation = Quaternion.Euler(_viewRotation * Time.fixedDeltaTime * 100);
        _rigid.MoveRotation(deltaRotation);
    }

    public void ResetCamera()
    {
        MoveCamera?.Invoke(_cameraHolder.position, _viewRotation);
    }
}
