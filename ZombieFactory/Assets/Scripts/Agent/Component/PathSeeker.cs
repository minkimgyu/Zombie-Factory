using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSeeker : MonoBehaviour, IInjectPathfind
{
    Func<Vector3, Vector3, List<Vector3>> FindPath;

    List<Vector3> _path;
    int _pathIndex = 0;
    const float _delayDuration = 0.8f;
    const float _reachDistance = 0.5f;

    Timer _delayTimer;

    public void AddPathfind(Func<Vector3, Vector3, List<Vector3>> FindPath)
    {
        this.FindPath = FindPath;
    }

    public void Initialize()
    {
        _storedTargetPos = Vector3.positiveInfinity;
        _delayTimer = new Timer();
    }

    //private void OnDrawGizmos()
    //{
    //    if (_path == null) return;

    //    for (int i = 1; i < _path.Count; i++)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(_path[i - 1], _path[i]);
    //    }
    //}

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
        bool notRunning = _delayTimer.CurrentState != Timer.State.Running;

        // 도착 거리보다 멀거나 타이머가 다 된 경우
        if (Vector3.Distance(targetPos, _storedTargetPos) >= _reachDistance && notRunning && FindPath != null)
        {
            _path = FindPath(transform.position, targetPos);
            _pathIndex = 0;

            _storedTargetPos = targetPos;
            _delayTimer.Reset();
            _delayTimer.Start(_delayDuration);
        }

        // 경로가 없는 경우
        if (_path == null || _path.Count == 0) return Vector3.zero;

        // 경로가 없거나 경로 끝에 도달한 경우 진행하지 않음
        float distance = Vector3.Distance(transform.position, _path[_pathIndex]);
        bool closeEnough = distance <= _reachDistance;
        if (closeEnough == true && _pathIndex < _path.Count - 1) _pathIndex++;

        return (_path[_pathIndex] - transform.position).normalized;
    }
}
