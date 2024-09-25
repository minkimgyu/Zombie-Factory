using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
abstract public class BaseMoveComponent : MonoBehaviour
{
    protected Rigidbody _rigid;
    [SerializeField] LayerMask _layerMask;

    public void Initialize()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    float maxSlopeAngle = 60f;

    bool _isSlope;
    RaycastHit _hitPoint;

    public void CheckIsOnSlope()
    {
        _isSlope = IsOnSlope(out _hitPoint);
    }

    bool IsOnSlope(out RaycastHit hitPoint)
    {
        RaycastHit hit;

        bool canHit = Physics.Raycast(_rigid.position + Vector3.up, Vector3.down, out hit, 1.25f, _layerMask);
        if (canHit == false)
        {
            hitPoint = hit;
            return false;
        }

        Debug.DrawLine(hit.point + Vector3.up, hit.point, Color.blue, 5f);
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.cyan, 5f);

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

        Debug.DrawLine(nextFramePlayerPosition, hit.point, Color.magenta, 5f);
        if (canHit == true)
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            //Debug.Log(angle);
            return angle < maxSlopeAngle;
        }

        return false;
    }

    public void Stop()
    {
        if (_isSlope)
        {
            _rigid.velocity = Vector3.zero;
        }
        else
        {
            _rigid.velocity = new Vector3(0, _rigid.velocity.y, 0);
        }
    }

    public virtual void Jump(float force) { }

    public virtual void Move(Vector3 direction, float speed)
    {
        //direction = transform.TransformVector(direction); // 먼저 diretion을 변형해줘야한다.

        bool canClimb = CanClimbSlope(direction, speed);
        if (canClimb == false) return;
        
        if (_isSlope)
        {
            direction = AdjustDirectionToSlope(direction, _hitPoint.normal);
            Debug.DrawRay(_hitPoint.point, direction, Color.red, 5f);
        }

        Vector3 moveDir = direction * speed;
        if (_isSlope)
        {
            _rigid.velocity = moveDir; // 방향 백터에 맞게 조절
        }
        else
        {
            _rigid.velocity = new Vector3(moveDir.x, _rigid.velocity.y, moveDir.z); // 경사로가 아닌 경우 _rigid.velocity.y를 적용해준다.
        }
    }
}
