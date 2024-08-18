using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapPoint : MonoBehaviour
{
    [SerializeField] Transform point;
    [SerializeField] Transform handTarget;

    // Update is called once per frame
    void Update()
    {
        handTarget.position = point.position;
        handTarget.rotation = point.rotation;
    }
}
