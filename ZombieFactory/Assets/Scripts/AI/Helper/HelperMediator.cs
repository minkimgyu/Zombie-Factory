using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����ڸ� ���� mediator
// �÷��̾�� �ش� ��ũ��Ʈ�� ������ ����� ������.
//
// ���� �� ��ġ �Ҵ�
// BuildFormation, FreeRole ����

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
