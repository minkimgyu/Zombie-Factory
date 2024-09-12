using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour, IPoint
{
    public Vector3 ReturnDirection() { return transform.forward; }
    public Vector3 ReturnPosition() { return transform.position; }
}
