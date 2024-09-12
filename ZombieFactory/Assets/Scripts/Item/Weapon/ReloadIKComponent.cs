using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ReloadIKComponent : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint _grapLeftHandIK;
    [SerializeField] TwoBoneIKConstraint _reloadLeftHandIK;
    [SerializeField] MultiParentConstraint _magazineMultiParent;

    [SerializeField] Transform _magazineGrabPoint; // 탄창의 위치 --> 지속적인 변경이 필요함

    //// 아래 두 변수를 총이 장착될 때 자동으로 할당해주게끔 하자
    //[SerializeField] Transform _magOriginPoint; // 총기에 탄창이 존재하는 원래 위치
    //[SerializeField] Transform _grapPoint; // 왼손이 탄창을 잡는 위치
    //[SerializeField] Transform _magazinePoint; // 탄창의 위치 --> 지속적인 변경이 필요함

    public void AssignIKPoints(Transform grapPointInGrip, Transform magOriginPoint, Transform grapPointInMag, Transform magazineObject)
    {
        return;

        _grapLeftHandIK.data.target = grapPointInGrip;
        _reloadLeftHandIK.data.target = grapPointInMag;
        _magazineMultiParent.data.constrainedObject = magazineObject;

        WeightedTransformArray weightedTransforms = new WeightedTransformArray();
        weightedTransforms.Add(new WeightedTransform(_magazineGrabPoint, 0));
        weightedTransforms.Add(new WeightedTransform(magOriginPoint, 1));

        _magazineMultiParent.data.sourceObjects.Clear();
        _magazineMultiParent.data.sourceObjects = weightedTransforms;
    }
}