using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnSightEnter();
    void OnSightExit();

    T ReturnComponent<T>(); // ������Ʈ ���� ��, �̰� ����ؼ� GetComponent ����
    bool IsInteractable();
}