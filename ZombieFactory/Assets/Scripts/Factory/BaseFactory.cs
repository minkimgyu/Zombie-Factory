using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFactory
{
    public virtual BaseLife Create(BaseLife.Name name) { return default; }
    public virtual BaseItem Create(BaseItem.Name name) { return default; }
    public virtual BaseEffect Create(BaseEffect.Name name) { return default; }

    public virtual Ragdoll Create(BaseLife.Name name, Vector3 pos, Quaternion rotation) { return default; }
    public virtual BaseLife Create(BaseLife.Name name, List<BaseItem.Name> weaponNames) { return default; }
}
