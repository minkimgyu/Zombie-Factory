using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieViewComponent : TPSViewComponent
{
    public override void Initialize(float viewYRange, Rigidbody rigidbody)
    {
        _rotation = Quaternion.identity;
        _rigidbody = rigidbody;
        _viewYRange = viewYRange;
    }
}
