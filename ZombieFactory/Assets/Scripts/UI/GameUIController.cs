using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] WeaponViewer _weaponViewer;
    [SerializeField] CrosshairViewer _crosshairViewer;
    [SerializeField] HpViewer _hpViewer;

    [SerializeField] AmmoViwer _ammoViewer;
    AmmoData _ammoData;

    public void Initialize()
    {
        _ammoData = new AmmoData(_ammoViewer);
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
        _ammoData.BulletCountInMagazine = inMagazine;
        _ammoData.BulletCountInPossession = inPossession;
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
