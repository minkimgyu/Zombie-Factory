using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnSightEnter();
    void OnSightExit();

    T ReturnComponent<T>(); // 오브젝트 리턴 후, 이걸 사용해서 GetComponent 진행
    bool IsInteractable();
}