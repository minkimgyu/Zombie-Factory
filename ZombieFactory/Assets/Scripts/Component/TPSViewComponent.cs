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
        _bone = animator.GetBoneTransform(HumanBodyBones.Spine); // �ش� ���� transform �������� --> �Ű� ������ �޾ƿ���
    }

    public override void RotateSpineBone()
    {
        _bone.rotation = Quaternion.Euler(_viewRotation) * Quaternion.Euler(ChestOffset); // ��ü �����̼� ����
    }
}
