using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBlackboard 
{
    public Action<BaseItem.Name, BaseWeapon.Type> AddPreview { get; private set; }
    public Action<BaseWeapon.Type> RemovePreview { get; private set; }

    public Action<bool> ActiveAmmoViewer { get; private set; }
    public Action<int, int> UpdateAmmoViewer { get; private set; }

    public Action<bool, float, Vector3, float> OnZoomRequested { get; private set; }
    public Action<string, int, float> OnPlayOwnerAnimation { get; private set; }
    public Func<float> SendMoveDisplacement { get; private set; }
    public Action<Vector2> OnRecoilRequested { get; private set; }

    public Transform AttackPoint { get; private set; }

    private WeaponBlackboard() { }

    public class Builder
    {
        public Builder()
        {
        }

        public Builder(WeaponBlackboard blackboard)
        {
            _blackboard = blackboard;
        }

        private readonly WeaponBlackboard _blackboard = new WeaponBlackboard();

        public Builder SetAddPreview(Action<BaseItem.Name, BaseWeapon.Type> AddPreview)
        {
            _blackboard.AddPreview = AddPreview;
            return this;
        }

        public Builder SetRemovePreview(Action<BaseWeapon.Type> RemovePreview)
        {
            _blackboard.RemovePreview = RemovePreview;
            return this;
        }

        public Builder SetActiveAmmoViewer(Action<bool> activeAmmoViewer)
        {
            _blackboard.ActiveAmmoViewer = activeAmmoViewer;
            return this;
        }

        public Builder SetUpdateAmmoViewer(Action<int, int> updateAmmoViewer)
        {
            _blackboard.UpdateAmmoViewer = updateAmmoViewer;
            return this;
        }

        public Builder SetOnZoomRequested(Action<bool, float, Vector3, float> onZoomRequested)
        {
            _blackboard.OnZoomRequested = onZoomRequested;
            return this;
        }

        public Builder SetOnPlayOwnerAnimation(Action<string, int, float> onPlayOwnerAnimation)
        {
            _blackboard.OnPlayOwnerAnimation = onPlayOwnerAnimation;
            return this;
        }

        public Builder SetSendMoveDisplacement(Func<float> sendMoveDisplacement)
        {
            _blackboard.SendMoveDisplacement = sendMoveDisplacement;
            return this;
        }

        public Builder SetOnRecoilRequested(Action<Vector2> onRecoilRequested)
        {
            _blackboard.OnRecoilRequested = onRecoilRequested;
            return this;
        }

        public Builder SetAttackPoint(Transform attackPoint)
        {
            _blackboard.AttackPoint = attackPoint;
            return this;
        }

        public WeaponBlackboard Build()
        {
            return _blackboard;
        }
    }
}
