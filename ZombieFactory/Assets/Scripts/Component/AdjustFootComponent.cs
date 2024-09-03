using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AdjustFootComponent : MonoBehaviour
{
    Animator animator;
    LayerMask hitMask;
    const float _distanceToGround = 0.1230242f;
    const float _startDistance = 0.2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        hitMask = LayerMask.GetMask("Ground");
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 leftFootIKPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFooIKPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);

        // �� �ٴڿ� ���� ������� ��� ������ �� �Ǳ� ������ ��¦ �÷��ش�.
        leftFootIKPos += Vector3.up * _startDistance;
        rightFooIKPos += Vector3.up * _startDistance;

        RaycastHit leftHit, rightHit;
        bool canHit;

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

        // distanceGround: LeftFoot���� �������� �Ÿ�
        // +1�� ���� ����: Vector3.up�� ���־��� ����
        canHit = Physics.Raycast(leftFootIKPos, Vector3.down, out leftHit, _distanceToGround + _startDistance, hitMask);
        if (canHit == true)
        {
            //Debug.DrawLine(leftFootIKPos, leftHit.point, Color.magenta, 5f);

            Vector3 footPosition = leftHit.point;
            footPosition.y += _distanceToGround;

            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            Vector3 projectionDir = Vector3.ProjectOnPlane(transform.forward, leftHit.normal);

            //Debug.DrawRay(footPosition, projectionDir * 2, Color.black, 5f);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(projectionDir, leftHit.normal));
        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

        canHit = Physics.Raycast(rightFooIKPos, Vector3.down, out rightHit, _distanceToGround + _startDistance, hitMask);
        if (canHit == true)
        {
            //Debug.DrawLine(rightFooIKPos, rightHit.point, Color.magenta, 5f);

            Vector3 footPosition = rightHit.point;
            footPosition.y += _distanceToGround;

            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
            Vector3 projectionDir = Vector3.ProjectOnPlane(transform.forward, rightHit.normal);

            //Debug.DrawRay(footPosition, projectionDir * 2, Color.black, 5f);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(projectionDir, rightHit.normal));
        }
    }
}
