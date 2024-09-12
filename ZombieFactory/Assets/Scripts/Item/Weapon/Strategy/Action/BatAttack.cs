using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MeleeAttack
{
    public BatAttack(BaseItem.Name weaponName, float range, int targetLayer, float delayForApplyDamage, DirectionData directionData) : base(weaponName, range, targetLayer, delayForApplyDamage, directionData)
    {
    }

    protected override void PlayMeleeAnimation()
    {
        PlayAnimation("Swing");
    }
}