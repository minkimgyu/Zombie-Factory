using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

abstract public class BaseEffect : MonoBehaviour, IPoolable
{
    public enum Name
    {
        PenetrateBulletHole, // �� ���� �� �Ѿ� �ڱ�
        NonPenetrateBulletHole, // �� ���� �� �Ѿ� �ڱ�


        WallFragmentation, // �� ���� ���� �� �Ѿ� ����ȭ
        KnifeMark, // Į�� �ڱ��� �� ���

        TrajectoryLine, // �Ѿ� �߻� ���� ����Ʈ
        Explosion, // ���� ���� ����Ʈ

        DamageTxt, // ������ �ؽ�Ʈ
        ObjectFragmentation // �����Ͽ� ������Ʈ�� �μ����� ���
    }

    [SerializeField] protected float _duration = 5;
    protected Timer _timer = new Timer();
    Action ReturnToPool;

    public virtual void Initialize() { }

    public virtual void ResetData(Vector3 hitPosition) { }

    public virtual void ResetData(Vector3 hitPosition, Vector3 shootPosition) { }

    public virtual void ResetData(Vector3 hitPosition, Vector3 hitNormal, Quaternion holeRotation) { }

    public virtual void ResetData(Vector3 hitPosition, Vector3 hitNormal, Quaternion holeRotation, float damamge) { }

    public virtual void Play()
    {
        _timer.Start(_duration);
    }

    protected virtual void Update()
    {
        if (_timer.CurrentState == Timer.State.Finish)
        {
            DisableObject();
        }
    }

    protected virtual void OnDisable()
    {
        _timer.Reset();
        ReturnToPool?.Invoke();
    }

    protected void DisableObject() => gameObject.SetActive(false);

    public void SetReturnToPoolEvent(Action ReturnToPool)
    {
        this.ReturnToPool = ReturnToPool;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public GameObject ReturnObject()
    {
        return gameObject;
    }
}
