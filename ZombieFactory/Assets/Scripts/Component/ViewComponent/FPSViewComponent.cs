using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FPSViewComponent : BaseViewComponent, IRecoilReceiver
{
    Transform _firePoint;
    [SerializeField] private Transform _armMesh;
    [SerializeField] private Transform _cameraHolder;

    [SerializeField] protected Vector3 _recoilForce;
    protected Vector2 _viewSensitivity = new Vector2(150, 150);

    [SerializeField] protected Vector3 _viewRotation;
    Vector2 FinalViewRotation { get { return _viewRotation + _recoilForce; } }
    Action<Vector3, Vector3> MoveCamera;

    public void AddObserverEvent(Action<Vector3, Vector3> MoveCamera)
    {
        this.MoveCamera = MoveCamera;
    }

    public override void Initialize(Transform firePoint, float viewYRange, Vector2 viewSensitivity, Rigidbody rigidbody)
    {
        _firePoint = firePoint;
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
        _viewSensitivity = viewSensitivity;
    }

    // 이 부분은 플레이어 컨트롤러에서 돌려주자
    public override void View(Vector2 dir)
    {
        float yRotation = _viewRotation.x - dir.y * _viewSensitivity.x * Time.deltaTime;
        yRotation = Mathf.Clamp(yRotation, -_viewYRange, _viewYRange);

        _viewRotation.x = yRotation;
        _viewRotation.y = _viewRotation.y + dir.x * _viewSensitivity.y * Time.deltaTime;
    }

    public override void RotateRigidbody()
    {
        _rigidbody.MoveRotation(Quaternion.Euler(0, FinalViewRotation.y, 0));
    }

    public void ResetCamera()
    {
        _firePoint.rotation = Quaternion.Euler(FinalViewRotation.x, FinalViewRotation.y, 0);
        _armMesh.rotation = Quaternion.Euler(FinalViewRotation.x, FinalViewRotation.y, 0);
        MoveCamera?.Invoke(_cameraHolder.position, FinalViewRotation);
    }

    public void OnRecoilRequested(Vector2 recoilForce)
    {
        _recoilForce = new Vector3(recoilForce.y, recoilForce.x);
    }
}
