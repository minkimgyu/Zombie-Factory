using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SwatViewComponent : TPSViewComponent, IRecoilReceiver
{
    Vector3 _recoilForce;
    Transform _attackObject;

    [SerializeField] Vector3 _chestOffset = new Vector3(0, 45, 0);
    [SerializeField] float _angleOffset = 10;
    Transform _bone;

    public override void Initialize(float viewYRange, Rigidbody rigidbody, Transform attackObject) 
    {
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
        _attackObject = attackObject;

        Animator animator = GetComponentInChildren<Animator>();
        _bone = animator.GetBoneTransform(HumanBodyBones.Spine); // �ش� ���� transform �������� --> �Ű� ������ �޾ƿ���
    }

    public override void RotateSpineBone()
    {
        float faceAngle = Vector3.SignedAngle(transform.forward, _dir, transform.right);

        Quaternion xRotation = Quaternion.AngleAxis(faceAngle + _angleOffset, Vector3.right); // x���� �������� ������.
        Vector3 spineRotation = _bone.localRotation.eulerAngles;

        _bone.localRotation = xRotation * Quaternion.Euler(_chestOffset) * _bone.localRotation;
    }

    Vector3 _dir;

    // �� �κ��� �÷��̾� ��Ʈ�ѷ����� ��������
    public override void View(Vector3 dir)
    {
        base.View(dir);
        if (dir == Vector3.zero) return;

        _dir = dir;
        Debug.DrawRay(_attackObject.position, _dir * 3, Color.yellow);
        Debug.DrawRay(_attackObject.position, transform.forward * 3, Color.red);
    }

    public void OnRecoilRequested(Vector2 recoilForce)
    {
        _recoilForce = new Vector3(recoilForce.y, recoilForce.x);
    }
}
