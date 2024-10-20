using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TPSViewComponent : BaseViewComponent
{
    [SerializeField] protected Vector3 ChestOffset = new Vector3(0, 45, 0);
    protected Transform _bone;

    Quaternion _rotation;

    public override void Initialize(float viewYRange, Rigidbody rigidbody)
    {
        _rotation = Quaternion.identity;
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;

        Animator animator = GetComponentInChildren<Animator>();
        _bone = animator.GetBoneTransform(HumanBodyBones.Spine); // �ش� ���� transform �������� --> �Ű� ������ �޾ƿ���
    }

    // �� �κ��� �÷��̾� ��Ʈ�ѷ����� ��������
    public override void View(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        _rotation = Quaternion.Lerp(_rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5);
    }

    public override void RotateSpineBone()
    {
        _bone.rotation = Quaternion.Euler(_viewRotation) * Quaternion.Euler(ChestOffset); // ��ü �����̼� ����
    }

    public override void RotateRigidbody()
    {
        if (_rotation == Quaternion.identity) return;

        _rigidbody.MoveRotation(_rotation);
    }
}
