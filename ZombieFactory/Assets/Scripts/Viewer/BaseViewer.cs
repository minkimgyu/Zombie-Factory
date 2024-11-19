using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseViewer : MonoBehaviour
{
    public enum Name
    {
        MoveableHpViewer,
        InfoViewer,
    }

    public virtual void ActiveViewer(bool active) { }
}