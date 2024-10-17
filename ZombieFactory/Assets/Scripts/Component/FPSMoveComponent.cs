using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMoveComponent : BaseMoveComponent
{
    public override void Jump(float force) 
    {
        _rigid.AddForce(transform.up * force, ForceMode.Impulse);
    }

    public override void Move(Vector3 direction, float speed)
    {
        Vector3 localDirection = transform.TransformVector(direction); // ���� diretion�� ����������Ѵ�.
        base.Move(localDirection, speed);
    }
}