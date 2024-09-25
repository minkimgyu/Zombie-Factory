using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseViewComponent : MonoBehaviour
{
    protected float _viewYRange;
    protected Vector3 _viewRotation;

    protected Rigidbody _rigidbody;

    public virtual void Initialize(float viewYRange) { }
    public virtual void Initialize(float viewYRange, Vector2 viewSensitivity) { }

    public virtual void View(Vector2 dir) { }
    public virtual void View(Vector3 dir) { }

    public abstract void RotateRigidbody();
    public virtual void RotateSpineBone() { }
}
