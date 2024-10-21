using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

// �����ڸ� ���� mediator
// �÷��̾�� �ش� ��ũ��Ʈ�� ������ ����� ������.
//
// ���� �� ��ġ �Ҵ�
// BuildFormation, FreeRole ����

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

        // �������� ������ ����
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
        float angleStep = 360f / pointCount; // �� ����Ʈ ������ ����

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * angleStep;
            float radian = angle * Mathf.Deg2Rad; // ������ �������� ��ȯ

            // x�� y ��ǥ ���
            float x = radius * Mathf.Cos(radian);
            float z = radius * Mathf.Sin(radian);
            points[i] = new Vector3(x, 0, z);
        }

        return points;
    }
}
