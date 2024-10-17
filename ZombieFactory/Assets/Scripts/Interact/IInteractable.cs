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

public interface IInteracter
{
    void GetWeapon(BaseWeapon weapon);
    void GetAmmoPack(int ammoCount);
    void GetAidPack(float healPoint);
}