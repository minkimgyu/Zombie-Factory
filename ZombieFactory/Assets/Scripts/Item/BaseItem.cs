using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseItem : MonoBehaviour
{
    public enum Name
    {
        AK,
        AR,
        AutoShotgun,
        SMG,
        DMR,
        Bat,
    }

    public virtual void Initialize() { }

    public virtual void ResetData(AutomaticGunData data, RecoilMapData recoilData, BaseFactory effectFactory) { }
    public virtual void ResetData(BatData data) { }
    public virtual void ResetData(JudgeData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(StingerData data, RecoilMapData mainMapData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(GuardianData data, RecoilRangeData mainRangeData, BaseFactory effectFactory) { }
}
