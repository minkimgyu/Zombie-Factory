using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseViewComponent : MonoBehaviour, IRecoilReceiver
{
    protected float _viewYRange;
    protected Vector3 _viewRotation;
    protected Vector3 _recoilForce;

    protected Rigidbody _rigidbody;

    public virtual void Initialize(float viewYRange, Rigidbody rigidbody) { }
    public virtual void Initialize(float viewYRange, Vector2 viewSensitivity, Rigidbody rigidbody) { }

    public virtual void View(Vector2 dir) { }
    public virtual void View(Vector3 dir) { }

    public abstract void RotateRigidbody();
    public virtual void RotateSpineBone() { }

    public void OnRecoilRequested(Vector2 recoilForce)
    {
        _recoilForce = new Vector3(recoilForce.y, recoilForce.x);
    }
}
