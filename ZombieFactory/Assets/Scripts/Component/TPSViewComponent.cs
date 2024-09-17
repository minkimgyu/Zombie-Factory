using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TPSViewComponent : ViewComponent
{
    [SerializeField] protected Vector3 ChestOffset = new Vector3(0, 45, 0);
    protected Transform _bone;

    public override void Initialize(float viewYRange, Vector2 viewSensitivity) 
    {
        base.Initialize(viewYRange, viewSensitivity);
        Animator animator = GetComponentInChildren<Animator>();
        _bone = animator.GetBoneTransform(HumanBodyBones.Spine); // 해당 본의 transform 가져오기 --> 매개 변수로 받아오기
    }

    public override void RotateSpineBone()
    {
        _bone.rotation = Quaternion.Euler(_viewRotation) * Quaternion.Euler(ChestOffset); // 상체 로테이션 보정
    }
}
