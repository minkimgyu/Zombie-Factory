using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zombie View와 Swat View를 나눠주자
abstract public class TPSViewComponent : BaseViewComponent
{
    protected Quaternion _yIgnoreRotation;
    
    public override void Initialize(float viewYRange, Rigidbody rigidbody)
    {
        _yIgnoreRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
    }

    // 이 부분은 플레이어 컨트롤러에서 돌려주자
    public override void View(Vector3 dir)
    {
        Vector3 rotationDir = dir;
        rotationDir.y = 0;
        if (rotationDir == Vector3.zero) return;
        _yIgnoreRotation = Quaternion.Lerp(_yIgnoreRotation, Quaternion.LookRotation(rotationDir, Vector3.up), Time.deltaTime * 5);
    }

    public override void RotateRigidbody()
    {
        if (_yIgnoreRotation == Quaternion.identity) return;

        _rigidbody.MoveRotation(_yIgnoreRotation.normalized);
    }
}
