using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairViewer : BaseViewer
{
    [SerializeField] GameObject _crosshair;

    public override void ActiveViewer(bool active)
    {
        _crosshair.SetActive(active);
    }
}
