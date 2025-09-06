using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSeeker : MonoBehaviour, IPathSeeker
{
    IPathfinder _pathfinder;

    List<Vector3> _path;
    int _pathIndex = 0;
    const float _delayDuration = 0.8f;
    const float _reachDistance = 0.5f;

    Timer _delayTimer;

    public void InjectPathfinder(IPathfinder pathfinder)
    {
        _pathfinder = pathfinder;
    }

    public void Initialize()
    {
        _storedTargetPos = Vector3.positiveInfinity;
        _delayTimer = new Timer();
    }

    public bool NowFinish()
    {
        if (_path != null && _pathIndex == _path.Count - 1)
        {
            return true;
        }

        return false;
    }

    Vector3 _storedTargetPos;

    public Vector3 ReturnDirection(Vector3 targetPos)
    {
        if( _pathfinder == null) return Vector3.zero;
        bool nowRunning = _delayTimer.CurrentState != Timer.State.Running;

        // ���� �Ÿ����� �ְų� Ÿ�̸Ӱ� �� �� ���
        if (Vector3.Distance(targetPos, _storedTargetPos) >= _reachDistance && nowRunning)
        {
            _path = _pathfinder.FindPath(transform.position, targetPos);
            _pathIndex = 0;

            _storedTargetPos = targetPos;
            _delayTimer.Reset();
            _delayTimer.Start(_delayDuration);
        }

        // ��ΰ� ���� ���
        if (_path == null || _path.Count == 0) return Vector3.zero;

        // ��ΰ� ���ų� ��� ���� ������ ��� �������� ����
        float distance = Vector3.Distance(transform.position, _path[_pathIndex]);
        bool closeEnough = distance <= _reachDistance;
        if (closeEnough == true && _pathIndex < _path.Count - 1) _pathIndex++;

        return (_path[_pathIndex] - transform.position).normalized;
    }
}
