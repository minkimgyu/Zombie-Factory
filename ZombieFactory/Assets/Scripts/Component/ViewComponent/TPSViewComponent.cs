using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zombie View�� Swat View�� ��������
abstract public class TPSViewComponent : BaseViewComponent
{
    protected Quaternion _yIgnoreRotation;
    
    public override void Initialize(float viewYRange, Rigidbody rigidbody)
    {
        _yIgnoreRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
    }

    // �� �κ��� �÷��̾� ��Ʈ�ѷ����� ��������
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
