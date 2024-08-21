using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    Rigidbody _rigid;
    [SerializeField] Transform _direction;
    [SerializeField] Animator _animator;

    Vector3 _storedDirection;

    public void Initialize()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Sit()
    {
        // 타이머 넣어서 CapsuleCollider 줄이기
        // PostureState 넣어서 해결하자
    }



    public void Move(Vector3 direction, float speed)
    {
        _storedDirection = Vector3.Lerp(_storedDirection, direction, Time.deltaTime * speed);

        _animator.SetFloat("Z", _storedDirection.z);
        _animator.SetFloat("X", _storedDirection.x);

        Vector3 moveDir = _direction.TransformVector(direction);
        moveDir = new Vector3(moveDir.x, 0, moveDir.z);
        // y축으로 움직이는 기능 제거

        _rigid.velocity = moveDir * speed;
    }
}
