using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseItem : MonoBehaviour
{
    public enum Name
    {
        Vandal, // -> O
        Phantom, // -> O
        Odin, // -> O

        Judge,
        Stinger, // -> O
        Guardian, // -> X
        Bucky,

        Operator, // -> X
        Classic, // -> O
        Knife, // -> O

        AmmoPack,
        AidPack
    }

    public virtual void PositionWeapon(bool nowDrop) { }

    public virtual void Initialize() { }

    public virtual void ResetData(AidPackData data) { }
    public virtual void ResetData(AmmoPackData data) { }

    public virtual void ResetData(AutomaticGunData data, RecoilMapData recoilData, BaseFactory effectFactory) { }
    public virtual void ResetData(OperatorData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(ClassicData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(BuckyData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(KnifeData data, BaseFactory effectFactory) { }
    public virtual void ResetData(JudgeData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(StingerData data, RecoilMapData mainMapData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(GuardianData data, RecoilRangeData mainRangeData, BaseFactory effectFactory) { }
}
