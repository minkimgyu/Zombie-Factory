using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseItem : MonoBehaviour
{
    public enum Name
    {
        Vandal,
        Phantom,
        Odin,

        Judge,
        Stinger,
        Guardian,
        Bucky,

        Operator,
        Classic,
        Knife
    }

    public virtual void Initialize() { }

    public virtual void ResetData(AutomaticGunData data, RecoilMapData recoilData, BaseFactory effectFactory) { }
    public virtual void ResetData(OperatorData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(ClassicData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(BuckyData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(KnifeData data, BaseFactory effectFactory) { }
    public virtual void ResetData(JudgeData data, RecoilRangeData mainRangeData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(StingerData data, RecoilMapData mainMapData, RecoilRangeData subRangeData, BaseFactory effectFactory) { }
    public virtual void ResetData(GuardianData data, RecoilRangeData mainRangeData, BaseFactory effectFactory) { }
}