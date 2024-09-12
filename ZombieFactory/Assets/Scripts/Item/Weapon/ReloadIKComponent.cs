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

    [SerializeField] Transform _magazineGrabPoint; // źâ�� ��ġ --> �������� ������ �ʿ���

    //// �Ʒ� �� ������ ���� ������ �� �ڵ����� �Ҵ����ְԲ� ����
    //[SerializeField] Transform _magOriginPoint; // �ѱ⿡ źâ�� �����ϴ� ���� ��ġ
    //[SerializeField] Transform _grapPoint; // �޼��� źâ�� ��� ��ġ
    //[SerializeField] Transform _magazinePoint; // źâ�� ��ġ --> �������� ������ �ʿ���

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