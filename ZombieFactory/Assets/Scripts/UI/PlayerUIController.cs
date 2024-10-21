using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] WeaponViewer _weaponViewer;
    [SerializeField] CrosshairViewer _crosshairViewer;
    [SerializeField] HpViewer _hpViewer;
    [SerializeField] AmmoViwer _ammoViewer;
    [SerializeField] TMP_Text _helperModeViewer;

    public void Initialize(Dictionary<BaseItem.Name, Sprite> weaponIconSprite)
    {
        _weaponViewer.Initialize(weaponIconSprite);
        OnFreeRoleRequested();
    }

    public void OnFreeRoleRequested()
    {
        _helperModeViewer.text = "FreeRole";
    }

    public void OnBuildFormationRequested()
    {
        _helperModeViewer.text = "BuildFormation";
    }

    public void ChangeHpRatio(float ratio)
    {
        _hpViewer.UpdateViewer(ratio);
    }

    public void ActiveCrosshair(bool active)
    {
        _crosshairViewer.ActiveViewer(active);
    }

    public void ChangeAmmo(int inMagazine, int inPossession)
    {
        _ammoViewer.ChangeAmmo(inMagazine, inPossession);
    }

    public void ActiveAmmoViewer(bool active)
    {
        _ammoViewer.ActiveViewer(active);
    }

    public void ActiveCrosshairViewer(bool active)
    {
        _crosshairViewer.ActiveViewer(active);
    }

    public void RemoveWeaponViewer(BaseWeapon.Type type)
    {
        _weaponViewer.RemovePreview(type);
    }

    public void AddWeaponViewer(BaseItem.Name name, BaseWeapon.Type type)
    {
        _weaponViewer.AddPreview(name, type);
    }
}
