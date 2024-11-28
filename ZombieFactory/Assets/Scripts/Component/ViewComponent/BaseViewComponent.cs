using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseViewComponent : MonoBehaviour
{
    protected float _viewYRange;
    protected Rigidbody _rigidbody;

    public virtual void Initialize(float viewYRange, Rigidbody rigidbody) { }
    public virtual void Initialize(float viewYRange, Rigidbody rigidbody, Transform viewObject) { }
    public virtual void Initialize(Transform firePoint, float viewYRange, Vector2 viewSensitivity, Rigidbody rigidbody) { }

    public virtual void View(Vector2 dir) { }
    public virtual void View(Vector3 dir) { }

    public virtual void RotateSpineBone() { }
    public abstract void RotateRigidbody();
}
