using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatViewComponent : BaseViewComponent, IRecoilReceiver
{
    Vector3 _recoilForce;
    Transform _attackPoint;

    Vector3 _chestOffset = new Vector3(0, 38.7f, 19f);
    Transform _spineBone;

    [SerializeField] float _rotateSpeed = 5;
    float _distanceFromPlayer = 1.5f;

    Quaternion _lowerBodyYRotation;
    Vector3 _upperBodyAimPoint;

    public override void Initialize(float viewYRange, Rigidbody rigidbody, Transform attackPoint) 
    {
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
        _attackPoint = attackPoint;

        _dir = rigidbody.transform.forward; // _dir �ʱ�ȭ

        _lowerBodyYRotation = Quaternion.LookRotation(rigidbody.transform.forward, Vector3.up);
        _upperBodyAimPoint = _attackPoint.position + rigidbody.transform.forward * _distanceFromPlayer;

        Animator animator = GetComponentInChildren<Animator>();
        _spineBone = animator.GetBoneTransform(HumanBodyBones.Spine); // �ش� ���� transform �������� --> �Ű� ������ �޾ƿ���
    }

    Vector3 _dir;

    public override void RotateSpineBone()
    {
        Vector3 rotationDir = _dir;
        rotationDir.y = 0;

        if (rotationDir != Vector3.zero)
        {
            _lowerBodyYRotation = Quaternion.Lerp(_lowerBodyYRotation, Quaternion.LookRotation(rotationDir, Vector3.up), Time.deltaTime * _rotateSpeed);
            _upperBodyAimPoint = Vector3.Lerp(_upperBodyAimPoint, _attackPoint.position + _dir * _distanceFromPlayer, Time.deltaTime * _rotateSpeed);
        }

        // lerp�ϰ� �����̰� �ٲ��ֱ�
        _attackPoint.LookAt(_upperBodyAimPoint, Vector3.up);
        _spineBone.LookAt(_upperBodyAimPoint, Vector3.up);
        _spineBone.Rotate(_chestOffset);
    }

    // �� �κ��� �÷��̾� ��Ʈ�ѷ����� ��������
    public override void View(Vector3 dir)
    {
        _dir = dir;
    }

    public void OnRecoilRequested(Vector2 recoilForce)
    {
        _recoilForce = new Vector3(recoilForce.y, recoilForce.x);
    }

    public override void RotateRigidbody()
    {
        if (_lowerBodyYRotation == Quaternion.identity) return;
        _rigidbody.MoveRotation(_lowerBodyYRotation.normalized);
    }
}
