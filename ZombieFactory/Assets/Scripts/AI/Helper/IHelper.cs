using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AI.Swat;

public interface IHelper
{
    void OnAddHelper(ITarget commander, Action<IHelper> RemoveHelper);

    void RestOffset(Vector3 offset); // mediator���� �޾ƿ�
    void GetAmmoPack(int ammoCount); // mediator���� �޾ƿ�
    void GetAidPack(float healPoint); // mediator���� �޾ƿ�
    void TeleportTo(Vector3 pos); // ��ġ �̵�


    void ChangeState(Swat.MovementState state);
}
