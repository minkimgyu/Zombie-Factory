using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct AmmoData
{
    BaseViewer _roundViwer;

    int bulletCountInMagazine;
    public int BulletCountInMagazine
    {
        get 
        {
            return bulletCountInMagazine; 
        }
        set
        {
            bulletCountInMagazine = value;
            _roundViwer.UpdateViewer(bulletCountInMagazine, bulletCountInPossession);
        }
    }

    int bulletCountInPossession;
    public int BulletCountInPossession
    {
        get
        {
            return bulletCountInPossession;
        }
        set
        {
            bulletCountInMagazine = value;
            _roundViwer.UpdateViewer(bulletCountInMagazine, bulletCountInPossession);
        }
    }

    public AmmoData(BaseViewer roundViwer)
    {
        _roundViwer = roundViwer;
        bulletCountInMagazine = 0;
        bulletCountInPossession = 0;
    }
}

public class AmmoViwer : BaseViewer
{
    [SerializeField] TMP_Text _bulletCountInMagazine;
    [SerializeField] TMP_Text _bulletCountInPossession;

    public override void UpdateViewer(int inMagazine, int inPossession)
    {
        _bulletCountInMagazine.text = inMagazine.ToString();
        _bulletCountInPossession.text = inPossession.ToString();
    }
}