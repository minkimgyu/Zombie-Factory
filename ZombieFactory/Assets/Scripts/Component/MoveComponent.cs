using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    Rigidbody _rigid;
    [SerializeField] Transform _direction;

    public void Initialize()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Stop()
    {
        _rigid.velocity = Vector3.zero;
    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 moveDir = _direction.TransformVector(direction);
        _rigid.velocity = moveDir * speed;
    }
}
