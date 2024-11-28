using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : LifePart
{
    public override void Initialize(IDamageable parentDamageable, ITarget parentBody, BaseFactory effectFactory)
    {
        base.Initialize(parentDamageable, parentBody, effectFactory);
        _durability = 1.5f;
        _area = IHitable.Area.Body;
    }
}
