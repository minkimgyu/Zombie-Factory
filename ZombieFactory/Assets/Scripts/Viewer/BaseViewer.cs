using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseViewer : MonoBehaviour
{
    public enum Name
    {

        HpViewer,
        ItemSlot
    }

    public virtual void ActiveViewer(bool active) { }


    public virtual void UpdateViewer(float value) { }
    public virtual void UpdateViewer(int current, int total) { }
    public virtual void UpdateViewer(float ratio, Color startColor, Color endColor) { }
    public virtual void UpdateViewer(BaseItem.Name name, BaseWeapon.Type type) { }


    public virtual void UpdateViewer(Sprite sprite) { }
}