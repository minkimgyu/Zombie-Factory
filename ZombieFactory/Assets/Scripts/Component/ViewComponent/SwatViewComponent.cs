using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SwatViewComponent : TPSViewComponent, IRecoilReceiver
{
    protected Vector3 _recoilForce;
    Transform _viewObject;

    Quaternion _yIKLookRotation;
    [SerializeField] protected Vector3 ChestOffset = new Vector3(0, 45, 0);
    protected Transform _bone;

    public override void Initialize(float viewYRange, Rigidbody rigidbody, Transform viewObject) 
    {
        _rotation = Quaternion.identity;
        _yIKLookRotation = Quaternion.identity;
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
        _viewObject = viewObject;

        Animator animator = GetComponentInChildren<Animator>();
        _bone = animator.GetBoneTransform(HumanBodyBones.Spine); // 해당 본의 transform 가져오기 --> 매개 변수로 받아오기
    }

    public override void RotateSpineBone()
    {
        _bone.rotation = _yIKLookRotation * Quaternion.Euler(ChestOffset); // 상체 로테이션 보정
    }

    //public override void View(Vector3 dir)
    //{
    //    base.View(dir);

    //    Vector3 rotationDir = dir;
    //    if (rotationDir == Vector3.zero) return;

    //    _viewObject.transform.rotation = 
    //        Quaternion.Lerp(_yIKLookRotation, Quaternion.LookRotation(rotationDir, Vector3.up), Time.deltaTime * 5);
    //}

    public void OnRecoilRequested(Vector2 recoilForce)
    {
        _recoilForce = new Vector3(recoilForce.y, recoilForce.x);
    }
}
