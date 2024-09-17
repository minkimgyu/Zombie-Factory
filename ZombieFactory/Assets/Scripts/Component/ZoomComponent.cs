using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZoomComponent : MonoBehaviour//, IObserver<GameObject, bool, float, float, float, float, bool>
{
    Vector3 _zoomPosition;
    float _fieldOfView;

    Timer _timer;

    [SerializeField] Transform _armMesh;
    Action<bool> SwitchCrosshair;
    Action<float, float> OnFieldOfViewChange;

    public void Initialize()
    {
        _timer = new Timer();
    }

    public void AddObserverEvent(Action<bool> SwitchCrosshair, Action<float, float> OnFieldOfViewChange)
    {
        this.OnFieldOfViewChange = OnFieldOfViewChange;
        this.SwitchCrosshair = SwitchCrosshair;
    }

    public void OnZoomCalled(bool nowTurnOn, float zoomDuration, Vector3 zoomPosition, float fieldOfView)
    {
        _zoomPosition = zoomPosition;
        _fieldOfView = fieldOfView;

        SwitchCrosshair?.Invoke(nowTurnOn);
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ActiveCrosshair, true);

        // �ٷ� Zoom���� ��
        if (zoomDuration == 0)
        {
            MoveCamera(_zoomPosition, _fieldOfView);
            return;
        }

        if (_timer.CurrentState == Timer.State.Running || _timer.CurrentState == Timer.State.Finish) _timer.Reset();
        _timer.Start(zoomDuration);
    }

    /// <summary>
    /// �ڷ�ƾ�� �ƴ� Update���� Ÿ�̸� �����鼭 �����Ű��
    /// 
    /// OnZoomCalled���� isInstant�� duration �����ؼ� �־��൵ �� ��
    /// �������� �޷��ִ� �� ����ҰŶ� On, Off ��� ���� �� �־ �� �� ����.
    /// </summary>
    public void OnUpdate()
    {
        if (_timer.CurrentState == Timer.State.Finish) return;
        MoveCamera(_zoomPosition, _fieldOfView, _timer.Ratio);
    }

    void MoveCamera(Vector3 armPosition, float fieldOfView, float progress = 1)
    {
        OnFieldOfViewChange?.Invoke(fieldOfView, progress);
        _armMesh.localPosition = Vector3.Lerp(_armMesh.localPosition, armPosition, progress);
    }
}
