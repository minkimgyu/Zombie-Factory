using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [SerializeField] BaseViewer _ammoViwer;
    AmmoData _ammoData;

    public void Initialize()
    {
        _ammoData = new AmmoData(_ammoViwer);
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.Type.ChageAmmoCount, new ChangeAmmoCommand(ChangeAmmo));
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.Type.ActiveAmmoViewer, new ActiveCommand(ActiveViewer));
    }

    public void ChangeAmmo(int inMagazine, int inPossession)
    {
        _ammoData.BulletCountInMagazine = inMagazine;
        _ammoData.BulletCountInPossession = inPossession;
    }

    public void ActiveViewer(bool active)
    {
        _ammoViwer.ActiveViewer(active);
    }
}
