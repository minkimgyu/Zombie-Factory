using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandGripComponent : MonoBehaviour
{
    Animator _animator;
    [SerializeField] Transform _gripPoint;
    bool _isActive;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AssignGripPoint(Transform gripPoint)
    {
        _gripPoint = gripPoint;
    }

    public void HoldGripPoint()
    {

    }

    public void ReleaseGripPoint()
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(_isActive)
        {

        }

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        print("IK Updated");

        _animator.SetIKPosition(AvatarIKGoal.LeftHand, _gripPoint.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, _gripPoint.rotation);
    }
}
