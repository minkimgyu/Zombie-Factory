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
    Transform playerSpineTr;
    [SerializeField] Vector3 ChestOffset = new Vector3(0, -40, -100);

    public enum FaceState
    {
        Foward,
        Right,
        Back,
        Left
    }

    [SerializeField] FaceState _faceState = FaceState.Foward;


    public void Initialize()
    {
        playerSpineTr = _animator.GetBoneTransform(HumanBodyBones.Spine); // �ش� ���� transform ��������
    }

    // �� �κ��� �÷��̾� ��Ʈ�ѷ����� ��������
    public void ResetView()
    {
        _viewRotation.y = _viewRotation.y + Input.GetAxisRaw("Mouse X") * _viewSensitivity.y * Time.deltaTime;
        _viewRotation.x = _viewRotation.x + Input.GetAxisRaw("Mouse Y") * _viewSensitivity.x * Time.deltaTime;

        _direction.rotation = Quaternion.Euler(0, _viewRotation.y, 0);
    }


    public void RotateRigidbody()
    {
        _rigidbody.MoveRotation(Quaternion.Euler(0, _viewRotation.y, 0));
    }

    public void RotateBody()
    {
        playerSpineTr.rotation = Quaternion.Euler(_viewRotation) * Quaternion.Euler(ChestOffset); // ��ü �����̼� ����
    }

    public void ResetCamera()
    {
        _cameraController.MoveCamera(_cameraHolder.position, _viewRotation);
    }
}
