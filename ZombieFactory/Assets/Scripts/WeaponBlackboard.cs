using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct WeaponBlackboard 
{
    public WeaponBlackboard(Action<bool, float, Vector3, float> OnZoomRequested, Func<float> SendMoveDisplacement,
        Action<Vector2> OnRecoilRequested, Action<string, int, float> OnPlayOwnerAnimation, IPoint attackPoint)
    {
        this.OnZoomRequested = OnZoomRequested;
        this.OnPlayOwnerAnimation = OnPlayOwnerAnimation;
        this.SendMoveDisplacement = SendMoveDisplacement;
        this.OnRecoilRequested = OnRecoilRequested;
        AttackPoint = attackPoint;
    }

    public Action<bool, float, Vector3, float> OnZoomRequested { get; }
    public Action<string, int, float> OnPlayOwnerAnimation { get; }
    public Func<float> SendMoveDisplacement { get; set; }
    public Action<Vector2> OnRecoilRequested { get; }

    public IPoint AttackPoint { get; }
}
