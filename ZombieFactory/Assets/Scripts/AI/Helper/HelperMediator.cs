using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조력자를 위해 mediator
// 플레이어에서 해당 스크립트를 가지고 명령을 내린다.
//
// 생성 시 위치 할당
// BuildFormation, FreeRole 적용

public class HelperMediator
{
    const int _distanceFromPlayer = 3;

    ITarget _player;
    List<IHelper> _helpers;

    void RemoveHelper(IHelper helper)
    {
        _helpers.Remove(helper);
    }

    public void OnAddHelper()
    {
        Vector2 pos = Random.insideUnitCircle * _distanceFromPlayer;
        for (int i = 0; i < _helpers.Count; i++)
        {
            _helpers[i].InitializeHelper(_player, RemoveHelper);
            _helpers[i].RestOffset(pos);
        }
    }

    public void BuildFormation()
    {
        Vector3[] points = GetCirclePoints(_distanceFromPlayer, _helpers.Count);
        for (int i = 0; i < _helpers.Count; i++)
        {
            _helpers[i].RestOffset(points[i]);
        }
    }

    Vector3[] GetCirclePoints(float radius, int pointCount)
    {
        Vector3[] points = new Vector3[pointCount];
        float angleStep = 360f / pointCount; // 각 포인트 사이의 각도

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * angleStep;
            float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환

            // x와 y 좌표 계산
            float x = radius * Mathf.Cos(radian);
            float z = radius * Mathf.Sin(radian);
            points[i] = new Vector3(x, 0, z);
        }

        return points;
    }
}
