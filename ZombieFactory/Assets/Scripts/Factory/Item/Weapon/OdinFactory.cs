using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinFactory : AutomaticGunCreater
{
    public OdinFactory(BaseItem prefab, ItemData data, BaseRecoilData recoilData, BaseFactory effectFactory) : base(prefab, data, recoilData, effectFactory)
    {
    }
}