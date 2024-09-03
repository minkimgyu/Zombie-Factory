using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGripComponent : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform gunHandPoint;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        //print("IK Updated");

        animator.SetIKPosition(AvatarIKGoal.LeftHand, gunHandPoint.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, gunHandPoint.rotation);
    }
}
