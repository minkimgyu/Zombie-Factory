using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zombie View�� Swat View�� ��������
abstract public class TPSViewComponent : BaseViewComponent
{
    protected Quaternion _rotation;

    public override void Initialize(float viewYRange, Rigidbody rigidbody)
    {
        _rotation = Quaternion.identity;
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
    }

    // �� �κ��� �÷��̾� ��Ʈ�ѷ����� ��������
    public override void View(Vector3 dir)
    {
        Vector3 rotationDir = dir;
        rotationDir.y = 0;
        if (rotationDir == Vector3.zero) return;

        _rotation = Quaternion.Lerp(_rotation, Quaternion.LookRotation(rotationDir, Vector3.up), Time.deltaTime * 5);
    }

    public override void RotateRigidbody()
    {
        if (_rotation == Quaternion.identity) return;

        _rigidbody.MoveRotation(_rotation);
    }
}
