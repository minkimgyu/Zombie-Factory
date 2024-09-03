using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureComponent<Target> : MonoBehaviour
{
    Action<Target> OnEnter; 
    Action<Target> OnExit;

    public void Initialize(Action<Target> OnEnter, Action<Target> OnExit)
    {
        this.OnEnter = OnEnter;
        this.OnExit = OnExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        Target target =  other.gameObject.GetComponent<Target>();
        //if (target as UnityEngine.Object == null) return;

        OnEnter?.Invoke(target);
    }

    private void OnTriggerExit(Collider other)
    {
        Target target = other.gameObject.GetComponent<Target>();
        //if (target as UnityEngine.Object == null) return;

        OnExit?.Invoke(target);
    }
}
