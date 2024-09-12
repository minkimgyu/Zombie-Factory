using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundViwer : BaseViewer
{
    [SerializeField] TMP_Text _bulletCountInMagazine;
    [SerializeField] TMP_Text _bulletCountInPossession;

    public override void UpdateViewer(int inMagazine, int inPossession)
    {
        _bulletCountInMagazine.text = inMagazine.ToString();
        _bulletCountInPossession.text = inPossession.ToString();
    }
}
