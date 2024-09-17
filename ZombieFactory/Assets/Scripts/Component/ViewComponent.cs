using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class ViewComponent : MonoBehaviour, IRecoilReceiver
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] protected Vector3 _viewRotation;

    Vector3 _recoilForce;
    Vector2 FinalViewRotation { get { return _viewRotation + _recoilForce; } }
    protected Vector2 _viewSensitivity = new Vector2(150, 150);
    protected Rigidbody _rigidbody;

    protected float _viewYRange;

    Action<Vector3, Vector3> MoveCamera;

    public void AddObserverEvent(Action<Vector3, Vector3> MoveCamera)
    {
        this.MoveCamera = MoveCamera;
    }

    public virtual void Initialize(float viewYRange, Vector2 viewSensitivity) 
    {
        _rigidbody = GetComponent<Rigidbody>();
        _viewYRange = viewYRange;
        _viewSensitivity = viewSensitivity;
    }

    // 이 부분은 플레이어 컨트롤러에서 돌려주자
    public void ResetView()
    {
        float yRotation = _viewRotation.x - Input.GetAxisRaw("Mouse Y") * _viewSensitivity.x * Time.deltaTime;
        yRotation = Mathf.Clamp(yRotation, -_viewYRange, _viewYRange);

        _viewRotation.x = yRotation;
        _viewRotation.y = _viewRotation.y + Input.GetAxisRaw("Mouse X") * _viewSensitivity.y * Time.deltaTime;
    }

    public void RotateRigidbody()
    {
        _rigidbody.MoveRotation(Quaternion.Euler(0, FinalViewRotation.y, 0));
    }

    public virtual void RotateSpineBone() { }

    public void ResetCamera()
    {
        _firePoint.rotation = Quaternion.Euler(FinalViewRotation.x, FinalViewRotation.y, 0);
        MoveCamera?.Invoke(_cameraHolder.position, FinalViewRotation);
    }

    public void OnRecoilRequested(Vector2 recoilForce)
    {
        _recoilForce = new Vector3(recoilForce.y, recoilForce.x);
    }
}
