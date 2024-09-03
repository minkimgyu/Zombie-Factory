using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class ViewComponent : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Transform _cameraHolder;

    [SerializeField] private Transform _direction;
    [SerializeField] Vector3 _viewRotation;
    [SerializeField] Vector2 _viewSensitivity;
    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] Animator _animator;
    [SerializeField] Vector3 ChestOffset = new Vector3(0, -40, -100);

    Transform _spineBorn;
    float _viewYRange;

    public void Initialize(float viewYRange, Vector2 viewSensitivity, Transform spineBorn)
    {
        _viewYRange = viewYRange;
        _viewSensitivity = viewSensitivity;
        _spineBorn = spineBorn; //_animator.GetBoneTransform(HumanBodyBones.Spine); // �ش� ���� transform �������� --> �Ű� ������ �޾ƿ���
    }

    // �� �κ��� �÷��̾� ��Ʈ�ѷ����� ��������
    public void ResetView()
    {
        float yRotation = _viewRotation.x + Input.GetAxisRaw("Mouse Y") * _viewSensitivity.x * Time.deltaTime;
        yRotation = Mathf.Clamp(yRotation, -_viewYRange, _viewYRange);

        _viewRotation.x = yRotation;
        _viewRotation.y = _viewRotation.y + Input.GetAxisRaw("Mouse X") * _viewSensitivity.y * Time.deltaTime;

        _direction.rotation = Quaternion.Euler(0, _viewRotation.y, 0);
    }

    public void RotateRigidbody()
    {
        _rigidbody.MoveRotation(Quaternion.Euler(0, _viewRotation.y, 0));
    }

    public void RotateSpineBone()
    {
        _spineBorn.rotation = Quaternion.Euler(_viewRotation) * Quaternion.Euler(ChestOffset); // ��ü �����̼� ����
    }

    public void ResetCamera()
    {
        _cameraController.MoveCamera(_cameraHolder.position, _viewRotation);
    }
}
