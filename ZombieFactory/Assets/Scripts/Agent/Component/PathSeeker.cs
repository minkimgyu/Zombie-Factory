using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSeeker : MonoBehaviour
{
    Func<Vector3, Vector3, List<Vector3>> FindPath;
    bool _onAir;

    List<Vector3> _path;
    int _pathIndex = 0;
    const float _reachDistance = 0.1f;


    public void Initialize(Func<Vector3, Vector3, List<Vector3>> FindPath, bool onAir)
    {
        this.FindPath = FindPath;
        _onAir = onAir;
        _storedTargetPos = Vector3.positiveInfinity;
    }

    private void OnDrawGizmos()
    {
        if (_path == null) return;

        for (int i = 1; i < _path.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_path[i - 1], _path[i]);
        }
    }

    public bool IsFinish()
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
        if (Vector3.Distance(targetPos, _storedTargetPos) > 0.5f)
        {
            Debug.Log("FindPath");
            _path = FindPath(transform.position, targetPos);
            _pathIndex = 0;
            _storedTargetPos = targetPos;
        }

        // ��ΰ� ���� ���
        if (_path == null || _path.Count == 0) return Vector3.zero;

        // ��ΰ� ���ų� ��� ���� ������ ��� �������� ����
        float distance = Vector3.Distance(transform.position, _path[_pathIndex]);
        bool closeEnough = distance < _reachDistance;
        if (closeEnough == true && _pathIndex < _path.Count - 1) _pathIndex++;

        return (_path[_pathIndex] - transform.position).normalized;
    }
}
