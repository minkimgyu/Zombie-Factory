using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Pistol, Shotgun�� ����
// LMG, AR, AK�� ���� �ӵ� ����, �ݵ� ���� --> Action, Recoil ��ȭ
// SMG�� �߻� ��� ����, ���� �ӵ� ����, �ݵ� ���� ---> Action, Recoil, Result ��ȭ
// Sniper�� ź ���� ��ȭ, ���� �ӵ� ��ȭ ---> Action, Recoil ��ȭ

abstract public class VariationGun : Gun
{
    public enum Conditon
    {
        Both, // ��� ���ǿ��� ����
        ZoomIn, // �� ���� �ÿ��� ����
        ZoomOut // �� ���� �ÿ��� ����
    }

    protected Dictionary<Tuple<EventType, Conditon>, EventStrategy> _eventStorage = new();
    protected Dictionary<Tuple<EventType, Conditon>, ActionStrategy> _actionStorage = new();
    protected Dictionary<Tuple<EventType, Conditon>, RecoilStrategy> _recoilStorage = new();

    public override void OnRooting(WeaponBlackboard blackboard)
    {
        base.OnRooting(blackboard);
        foreach (var eventState in _eventStorage) eventState.Value.LinkEvent(blackboard);
        foreach (var actionState in _actionStorage) actionState.Value.LinkEvent(blackboard);
        foreach (var recoilState in _recoilStorage) recoilState.Value.LinkEvent(blackboard);
    }

    public override void OnDrop(WeaponBlackboard blackboard)
    {
        base.OnDrop(blackboard);
        foreach (var eventState in _eventStorage) eventState.Value.UnlinkEvent(blackboard);
        foreach (var actionState in _actionStorage) actionState.Value.UnlinkEvent(blackboard);
        foreach (var recoilState in _recoilStorage) recoilState.Value.UnlinkEvent(blackboard);
    }

    // state �������� �ٲ��ֱ�
    protected void OnZoomRequested(bool nowZoom)
    {
        if (nowZoom) OnZoomIn();
        else OnZoomOut();
    }

    protected virtual void OnZoomIn() 
    {
        Tuple<EventType, Conditon> zoomInKey = new(EventType.Main, Conditon.ZoomIn);
        if (_eventStorage.ContainsKey(zoomInKey)) _eventStrategy[EventType.Main] = _eventStorage[zoomInKey];
        if (_actionStorage.ContainsKey(zoomInKey)) _actionStrategy[EventType.Main] = _actionStorage[zoomInKey];
        if (_recoilStorage.ContainsKey(zoomInKey)) _recoilStrategy[EventType.Main] = _recoilStorage[zoomInKey];
    }

    protected virtual void OnZoomOut() 
    {
        Tuple<EventType, Conditon> zoomOutKey = new(EventType.Main, Conditon.ZoomOut);
        if (_eventStorage.ContainsKey(zoomOutKey)) _eventStrategy[EventType.Main] = _eventStorage[zoomOutKey];
        if (_actionStorage.ContainsKey(zoomOutKey)) _actionStrategy[EventType.Main] = _actionStorage[zoomOutKey];
        if (_recoilStorage.ContainsKey(zoomOutKey)) _recoilStrategy[EventType.Main] = _recoilStorage[zoomOutKey];
    }

    public override void MatchStrategy()
    {
        Tuple<EventType, Conditon> mainBothKey = new(EventType.Main, Conditon.Both);
        Tuple<EventType, Conditon> subBothKey = new(EventType.Sub, Conditon.Both);

        Tuple<EventType, Conditon> mainZoomOutKey = new(EventType.Main, Conditon.ZoomOut);
        Tuple<EventType, Conditon> subZoomOutKey = new(EventType.Sub, Conditon.ZoomOut);


        if (_eventStorage.ContainsKey(mainBothKey)) _eventStrategy[EventType.Main] = _eventStorage[mainBothKey];
        else _eventStrategy[EventType.Main] = _eventStorage[mainZoomOutKey];

        if (_eventStorage.ContainsKey(subBothKey)) _eventStrategy[EventType.Sub] = _eventStorage[subBothKey];
        else _eventStrategy[EventType.Sub] = _eventStorage[subZoomOutKey];


        if (_actionStorage.ContainsKey(mainBothKey)) _actionStrategy[EventType.Main] = _actionStorage[mainBothKey];
        else _actionStrategy[EventType.Main] = _actionStorage[mainZoomOutKey];

        if (_actionStorage.ContainsKey(subBothKey)) _actionStrategy[EventType.Sub] = _actionStorage[subBothKey];
        else _actionStrategy[EventType.Sub] = _actionStorage[subZoomOutKey];


        if (_recoilStorage.ContainsKey(mainBothKey)) _recoilStrategy[EventType.Main] = _recoilStorage[mainBothKey];
        else _recoilStrategy[EventType.Main] = _recoilStorage[mainZoomOutKey];

        if (_recoilStorage.ContainsKey(subBothKey)) _recoilStrategy[EventType.Sub] = _recoilStorage[subBothKey];
        else _recoilStrategy[EventType.Sub] = _recoilStorage[subZoomOutKey];
    }
}