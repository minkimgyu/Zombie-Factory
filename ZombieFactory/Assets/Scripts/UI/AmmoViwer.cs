using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoViwer : BaseViewer
{
    [SerializeField] GameObject _ammoParent;
    [SerializeField] TMP_Text _bulletCountInMagazine;
    [SerializeField] TMP_Text _bulletCountInPossession;

    public override void ActiveViewer(bool active) 
    {
        _ammoParent.SetActive(active);
    }

    public void ChangeAmmo(int inMagazine, int inPossession)
    {
        _bulletCountInMagazine.text = inMagazine.ToString();
        _bulletCountInPossession.text = inPossession.ToString();
    }
}