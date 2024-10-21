using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

// 조력자를 위해 mediator
// 플레이어에서 해당 스크립트를 가지고 명령을 내린다.
//
// 생성 시 위치 할당
// BuildFormation, FreeRole 적용

public class HelperMediator
{
    const float _distanceFromPlayerInFreeRoleState = 4;
    const float _distanceFromPlayerInBuildFormationState = 2;
    const float _offsetRange = 2f;

    ITarget _player;
    public ITarget Player
    {
        get { return _player; }
    }

    List<IHelper> _helpers;

    Action OnFreeRoleRequested;
    Action OnBuildFormationRequested;

    public HelperMediator(Action OnFreeRoleRequested, Action OnBuildFormationRequested)
    {
        _helpers = new List<IHelper>();
        this.OnFreeRoleRequested = OnFreeRoleRequested;
        this.OnBuildFormationRequested = OnBuildFormationRequested;
    }

    void RemoveHelper(IHelper helper)
    {
        _helpers.Remove(helper);
    }

    public void AddPlayer(ITarget target)
    {
        _player = target;
    }

    public void AddHelper(IHelper helper)
    {
        helper.OnAddHelper(_player, RemoveHelper);
        _helpers.Add(helper);
        
        for (int i = 0; i < _helpers.Count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * _distanceFromPlayerInFreeRoleState;
            Vector3 pos = new Vector3(offset.x, 0, offset.y);
            _helpers[i].RestOffset(pos);
        }
    }

    public void BuildFormation()
    {
        OnBuildFormationRequested?.Invoke();
        Vector3[] points = GetCirclePoints(_distanceFromPlayerInBuildFormationState, _helpers.Count);

        // 원형으로 포지션 적용
        for (int i = 0; i < _helpers.Count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * _offsetRange;
            Vector3 pos = points[i] + new Vector3(offset.x, 0, offset.y);
            _helpers[i].RestOffset(pos);
            _helpers[i].ChangeState(AI.Swat.Swat.MovementState.BuildFormation);
        }
    }

    public void FreeRole()
    {
        OnFreeRoleRequested?.Invoke();
        Vector3[] points = GetCirclePoints(_distanceFromPlayerInFreeRoleState, _helpers.Count);

        for (int i = 0; i < _helpers.Count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * _offsetRange;
            Vector3 pos = points[i] + new Vector3(offset.x, 0, offset.y);
            _helpers[i].RestOffset(pos);
            _helpers[i].ChangeState(AI.Swat.Swat.MovementState.FreeRole);
        }
    }

    public void GetAmmoPack(int ammoCount)
    {
        for (int i = 0; i < _helpers.Count; i++)
        {
            _helpers[i].GetAmmoPack(ammoCount);
        }
    }

    public void GetAidPack(float healPoint)
    {
        for (int i = 0; i < _helpers.Count; i++)
        {
            _helpers[i].GetAidPack(healPoint);
        }
    }

    public void TeleportTo(Vector3 pos)
    {
        Vector3[] points = GetCirclePoints(_distanceFromPlayerInBuildFormationState, _helpers.Count);
        for (int i = 0; i < _helpers.Count; i++)
        {
            _helpers[i].TeleportTo(pos + points[i]);
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
