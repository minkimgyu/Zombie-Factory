using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AI.Swat;

public interface IHelper
{
    void OnAddHelper(ITarget commander, Action<IHelper> RemoveHelper);

    void RestOffset(Vector3 offset); // mediator에서 받아옴
    void GetAmmoPack(int ammoCount); // mediator에서 받아옴
    void GetAidPack(float healPoint); // mediator에서 받아옴
    void TeleportTo(Vector3 pos); // 위치 이동


    void ChangeState(Swat.MovementState state);
}
