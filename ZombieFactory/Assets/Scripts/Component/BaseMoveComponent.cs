using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using System;

//[RequireComponent(typeof(Rigidbody))]
abstract public class BaseMoveComponent : MonoBehaviour
{
    protected Rigidbody _rigid;
    [SerializeField] LayerMask _layerMask;

    Action<Vector3> ResetAnimator;

    public void Initialize(Rigidbody rigidbody, Action<Vector3> ResetAnimator = null)
    {
        _rigid = rigidbody;
        this.ResetAnimator = ResetAnimator;
    }

    float maxSlopeAngle = 70f;

    bool _isSlope;
    RaycastHit _hitPoint;

    public void TeleportTo(Vector3 pos)
    {
        _rigid.position = pos;
    }

    public void CheckIsOnSlope()
    {
        _isSlope = IsOnSlope(out _hitPoint);
    }

    bool IsOnSlope(out RaycastHit hitPoint)
    {
        RaycastHit hit;

        Vector3 nextFramePlayerPosition = _rigid.position + Vector3.up + _storedDirection * _storedSpeed * Time.fixedDeltaTime;
        bool canHit = Physics.Raycast(nextFramePlayerPosition, Vector3.down, out hit, 1.25f, _layerMask);
        if (canHit == false)
        {
            hitPoint = hit;
            return false;
        }

        //Debug.DrawLine(hit.point + Vector3.up, hit.point, Color.blue, 5f);
        //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.cyan, 5f);

        float angle = Vector3.Angle(Vector3.up, hit.normal);
        hitPoint = hit;
        return angle != 0f && angle < maxSlopeAngle;
    }

    Vector3 AdjustDirectionToSlope(Vector3 direction, Vector3 nomal)
    {
        return Vector3.ProjectOnPlane(direction, nomal);
    }

    bool CanClimbSlope(Vector3 direction, float speed)
    {
        Vector3 nextFramePlayerPosition = _rigid.position + Vector3.up + direction * speed * Time.fixedDeltaTime;

        RaycastHit hit;
        bool canHit = Physics.Raycast(nextFramePlayerPosition, Vector3.down, out hit, 3f, _layerMask);

        //Debug.DrawLine(nextFramePlayerPosition, hit.point, Color.magenta, 5f);
        if (canHit == true)
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            return angle < maxSlopeAngle;
        }

        return false;
    }

    public virtual void Jump(float force) { }

    Vector3 _storedDirection;
    float _storedSpeed;

    public void Stop()
    {
        ResetAnimator?.Invoke(Vector3.zero);

        if (_isSlope)
        {
            _storedDirection = Vector3.zero;

            if (_rigid.velocity.y > 0) // 현재 y 속도가 양수라면
            {
                _storedDirection = Vector3.zero; // 강제로 멈춰줌
            }
            else
            {
                _storedDirection = new Vector3(0, _rigid.velocity.y, 0);
            }
        }
        else
        {
            _storedDirection = new Vector3(0, _rigid.velocity.y, 0);
        }

        ResetAnimator?.Invoke(_storedDirection);
    }

    public virtual void Move(Vector3 direction, float speed, bool onAir = false)
    {
        if(onAir)
        {
            Vector3 moveDir = direction * speed;
            _storedDirection = new Vector3(moveDir.x, _rigid.velocity.y, moveDir.z); // 경사로가 아닌 경우 _rigid.velocity.y를 적용해준다.
        }
        else
        {
            _storedSpeed = speed;
            bool canClimb = CanClimbSlope(direction, speed);
            if (canClimb == false) return;

            if (_isSlope)
            {
                direction = AdjustDirectionToSlope(direction, _hitPoint.normal);
            }

            Vector3 moveDir = direction * speed;
            if (_isSlope)
            {
                _storedDirection = moveDir; // 방향 백터에 맞게 조절
            }
            else
            {
                _storedDirection = new Vector3(moveDir.x, _rigid.velocity.y, moveDir.z); // 경사로가 아닌 경우 _rigid.velocity.y를 적용해준다.
            }
        }

        ResetAnimator?.Invoke(_storedDirection);
    }

    public void MoveRigidbody()
    {
        _rigid.velocity = _storedDirection;
    }
}
