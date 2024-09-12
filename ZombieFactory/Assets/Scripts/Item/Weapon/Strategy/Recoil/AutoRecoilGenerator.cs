using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRecoilGenerator : RecoilGenerator
{
    int _index = -1;
    int _indexMultiplier = 1;
    int _repeatIndex = 0;

    float _recoilRatio;
    List<Vector2> _recoilPoints = new List<Vector2>();

    public AutoRecoilGenerator(float shootInterval, float recoveryDuration, float recoilRatio, RecoilMapData recoilData)
        : base(shootInterval, recoveryDuration)
    {
        _shootIntervalDuration = shootInterval;

        _recoilRatio = recoilRatio;
        _repeatIndex = recoilData.RepeatIndex;
        
        List<SerializableVector2> points = recoilData.ReturnAllAnglesBetweenCenterAndPoint();
        for (int i = 0; i < points.Count; i++) _recoilPoints.Add(points[i].V2);
    }

    protected override Vector2 ReturnNextRecoilPoint()
    {
        _index += _indexMultiplier;

        if (_index >= _recoilPoints.Count - 1) _indexMultiplier = -1; // �������� ������ ���
        else if (_indexMultiplier == -1 && _index == _repeatIndex) _indexMultiplier = 1;
        // ��ȯ �������� _indexMultiplier�� -1�� ���

        return _recoilPoints[_index] * _recoilRatio;
    }

    protected override void OnStartRecovering() { _index = -1; _indexMultiplier = 1; } // �ε��� �ʱ�ȭ �ʿ�
}
