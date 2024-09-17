using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : HitPoint
{
    public override void Initialize(IDamageable parentDamageable, IPoint parentBody, BaseFactory effectFactory)
    {
        base.Initialize(parentDamageable, parentBody, effectFactory);
        _durability = 1.5f;
        _area = IHitable.Area.Head;
    }
}