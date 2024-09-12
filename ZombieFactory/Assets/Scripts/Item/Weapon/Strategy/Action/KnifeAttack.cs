using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 칼 공격은 여기서 구현
abstract public class MeleeAttack : ApplyAttack
{
    float _delayForApplyDamage;
    Timer _delayTimer; // 공격 시 작동

    public MeleeAttack(BaseItem.Name weaponName, float range, int targetLayer, float delayForApplyDamage, DirectionData directionData)
        : base(weaponName, range, targetLayer)
    {
        _damageConverter = new DirectionBasedDamageConverter(directionData);

        _delayForApplyDamage = delayForApplyDamage;
        _delayTimer = new Timer();
    }

    public override void UnlinkEvent(WeaponBlackboard blackboard)
    {
        //ReturnRaycastPos -= blackboard.ReturnRaycastPos;
        //ReturnRaycastDir -= blackboard.ReturnRaycastDir;
        _attackPoint = null;
        OnPlayOwnerAnimation -= blackboard.OnPlayOwnerAnimation;
    }

    public override void LinkEvent(WeaponBlackboard blackboard)
    {
        //ReturnRaycastPos += blackboard.ReturnRaycastPos;
        //ReturnRaycastDir += blackboard.ReturnRaycastDir;
        _attackPoint = blackboard.AttackPoint;
        OnPlayOwnerAnimation += blackboard.OnPlayOwnerAnimation;
    }

    protected override float CalculateDamage(IHitable hitable)
    {
        Vector3 camFowardDir = _attackPoint.ReturnDirection();
        return _damageConverter.ReturnDamage(camFowardDir, Vector3.zero);
    }

    protected override void ApplyDamage(IHitable hitable, RaycastHit hit)
    {
        float damage = CalculateDamage(hitable);
        hitable.OnHit(damage, hit.point, hit.normal);
    }

    protected abstract void PlayMeleeAnimation();

    protected void Stab()
    {
        Vector3 camPos = _attackPoint.ReturnPosition();
        Vector3 camFowardDir = _attackPoint.ReturnDirection();

        //PlaySound?.Invoke(SoundType.Attack, true);

        RaycastHit hit;
        Physics.Raycast(camPos, camFowardDir, out hit, _range, _targetLayer);
        if (hit.collider == null) return;

        IEffectable effectable = hit.collider.GetComponent<IEffectable>();
        if (effectable == null) return;

        effectable.CreateEffect(IEffectable.ConditionType.Stabbing, hit.point, hit.normal);

        IHitable hitable = hit.collider.GetComponent<IHitable>();
        if (hitable == null) return;

        ApplyDamage(hitable, hit);
    }

    public override void OnUpdate()
    {
        if (_delayTimer.CurrentState != Timer.State.Finish) return;

        Stab();
        _delayTimer.Reset();
    }

    public override void Execute()
    {
        PlayMeleeAnimation();
        _delayTimer.Start(_delayForApplyDamage);
    }
}

public class RightKnifeAttack : MeleeAttack
{
    public RightKnifeAttack(BaseItem.Name weaponName, float range, int targetLayer,
        float delayForNextStab, DirectionData directionData)
        : base(weaponName, range, targetLayer, delayForNextStab, directionData)
    {
    }

    protected override void PlayMeleeAnimation() => PlayAnimation("Stab");
}

public class LeftKnifeAttack : MeleeAttack
{
    float _stabLinkDuration = 2.8f;
    int _stabIndex = 0;

    Timer _stabLinkTimer;

    int _animationCount;

    public LeftKnifeAttack(BaseItem.Name weaponName, float range, int targetLayer,
        int animationCnt, float delayForNextStab, float attackLinkDuration, DirectionData directionData)
        : base(weaponName, range, targetLayer, delayForNextStab, directionData)
    {
        _stabLinkDuration = attackLinkDuration;

        _animationCount = animationCnt;

        _stabLinkTimer = new Timer();
    }

    public override void Execute()
    {
        base.Execute();
        _stabLinkTimer.Start(_stabLinkDuration);
    }

    protected override void PlayMeleeAnimation()
    {
        if (_stabLinkTimer.CurrentState == Timer.State.Running)
        {
            _stabIndex++;
            if (_stabIndex > _animationCount - 1) _stabIndex = 0;
        }
        else
        {
            _stabIndex = 0;
        }

        _stabLinkTimer.Reset();
        PlayAnimation("Stab", _stabIndex);
    }
}