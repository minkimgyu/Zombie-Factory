using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZoomComponent : MonoBehaviour//, IObserver<GameObject, bool, float, float, float, float, bool>
{
    Vector3 _zoomPosition;
    float _fieldOfView;

    Timer _timer;

    //Action<bool> SwitchCrosshair;
    //Action<float, float> OnFieldOfViewChangeRequested;

    public void Initialize()
    {
        //CameraController controller = FindObjectOfType<CameraController>();
        //OnFieldOfViewChangeRequested = controller.OnFieldOfViewChangeRequested;

        _timer = new Timer();

        //CrosshairViewer crosshairController = FindObjectOfType<CrosshairViewer>();
        //if (crosshairController == null) return;
        //SwitchCrosshair = crosshairController.SwitchCrosshair;
    }

    public void OnZoomCalled(bool nowTurnOn, float zoomDuration, Vector3 zoomPosition, float fieldOfView)
    {
        _zoomPosition = zoomPosition;
        _fieldOfView = fieldOfView;

        //SwitchCrosshair?.Invoke(nowTurnOn);
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
        //OnFieldOfViewChangeRequested?.Invoke(fieldOfView, progress);
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.Type.ChangeFieldOfView, fieldOfView, progress);
        //_armMesh.localPosition = Vector3.Lerp(_armMesh.localPosition, armPosition, progress);
    }
}
