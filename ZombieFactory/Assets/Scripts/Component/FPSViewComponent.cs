using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSViewComponent : ViewComponent
{
    [SerializeField] Transform _bone;

    public override void RotateSpineBone()
    {
        _bone.rotation = Quaternion.Euler(_viewRotation);
    }
}
