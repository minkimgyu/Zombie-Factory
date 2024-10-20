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
        _bone = animator.GetBoneTransform(HumanBodyBones.Spine); // 해당 본의 transform 가져오기 --> 매개 변수로 받아오기
    }

    // 이 부분은 플레이어 컨트롤러에서 돌려주자
    public override void View(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        _rotation = Quaternion.Lerp(_rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5);
    }

    public override void RotateSpineBone()
    {
        _bone.rotation = Quaternion.Euler(_viewRotation) * Quaternion.Euler(ChestOffset); // 상체 로테이션 보정
    }

    public override void RotateRigidbody()
    {
        if (_rotation == Quaternion.identity) return;

        _rigidbody.MoveRotation(_rotation);
    }
}
