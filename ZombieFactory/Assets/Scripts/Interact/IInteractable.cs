using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(IInteracter interacter);

    void OnSightEnter();
    void OnSightExit();
    bool IsInteractable();
}

public interface IInteracter : IWeaponEquipable
{
    void TeleportTo(Vector3 pos);
    void GetAmmoPack(int ammoCount);
    void GetAidPack(float healPoint);
    void MovePosition(Vector3 position);
}