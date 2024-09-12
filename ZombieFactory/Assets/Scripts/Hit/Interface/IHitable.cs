using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public enum Area
    {
        Head,
        Body,
        Leg,
    }

    void OnHit(float damage, Vector3 hitPosition, Vector3 hitNormal);
    Area ReturnArea();
}
