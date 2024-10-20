using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightComponent : CaptureComponent<ITarget>
{
    float _captureRadius = 5;
    float _captureAngle = 90;
    [SerializeField] Transform _sightPoint;

    ITarget _target;

    protected List<ITarget> _capturedTargets = new List<ITarget>();

    List<IIdentifiable.Type> _targetTypes = new List<IIdentifiable.Type>();

    public void SetUp(float radius, float angle, List<IIdentifiable.Type> targetTypes)
    {
        _captureRadius = radius;
        _captureAngle = angle;
        _targetTypes = targetTypes;
        Initialize(OnEnter, OnExit);
        
    }

    void OnEnter(ITarget target)
    {
        bool isOpponent = target.IsOpponent(_targetTypes);
        if (isOpponent == false) return;

        _capturedTargets.Add(target);
    }

    void OnExit(ITarget target)
    {
        bool isOpponent = target.IsOpponent(_targetTypes);
        if (isOpponent == false) return;

        _capturedTargets.Remove(target);
    }

    bool CanRaycastTarget(Vector3 sightPoint, ITarget target)
    {
        Vector3 targetPos = target.ReturnPosition();
        targetPos = new Vector3(targetPos.x, sightPoint.y, targetPos.z);
        Vector3 dir = (targetPos - sightPoint).normalized;

        RaycastHit hit;
        Physics.Raycast(_sightPoint.position, dir, out hit, _captureRadius, _layerMask);
        if (hit.collider == null) return false;
        
        ITarget findTarget = hit.transform.GetComponent<ITarget>();
        if (findTarget == null) return false;

        if (findTarget == target)
        {
            Debug.DrawRay(_sightPoint.position, dir * _captureRadius, Color.yellow);
            return true;
        }
        else return false;
    }

    bool IsInAngle(float angle) { return angle <= _captureAngle / 2 && -_captureAngle / 2 <= angle; }

    public ITarget ReturnTargetInSight() { return _target; }

    public bool IsTargetInSight()
    {
        if (_capturedTargets.Count == 0) return false;

        for (int i = 0; i < _capturedTargets.Count; i++)
        {
            ITarget target = _capturedTargets[i];
            if (target as UnityEngine.Object == null)
            {
                _capturedTargets.Remove(target);
                i--;
                continue;
            }

            float angle = ReturnAngleBetween(target.ReturnPosition());
            bool inInAngle = IsInAngle(angle);
            if (inInAngle == false) continue;

            bool canRaycast = CanRaycastTarget(_sightPoint.position, target);
            if (canRaycast == false) continue;

            _target = _capturedTargets[i];
            return true;
        }

        return false;
    }

    float ReturnAngleBetween(Vector3 targetPos)
    {
        Vector3 dir = (new Vector3(targetPos.x, transform.position.y, targetPos.z) - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);
        return angle;
    }
}
