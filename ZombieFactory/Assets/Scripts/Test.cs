using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform _playerChest;
    [SerializeField] Transform _target;

    [SerializeField] Transform _attackPoint;

    [SerializeField] Vector3 _chestOffset = new Vector3(0, 45, 0);
    Transform _spineBone;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Animator animator = GetComponentInChildren<Animator>();
        animator.Play("VandalIdle", 2);
        _spineBone = animator.GetBoneTransform(HumanBodyBones.Spine); // 해당 본의 transform 가져오기 --> 매개 변수로 받아오기
    }

    Quaternion _yRotation;
    Rigidbody _rigidbody;
    [SerializeField] float _rotateSpeed = 5;

    Vector3 _viewNomalDir;
    float _distanceFromPlayer = 1.5f;

    private void Update()
    {
        float viewDistance = Vector3.Distance(_target.position, _playerChest.position);
        Vector3 viewDir = _target.position - _playerChest.position;
        _viewNomalDir = viewDir.normalized;

        Debug.DrawRay(_playerChest.position, _viewNomalDir * viewDistance, Color.red);

        Vector3 chestFowardDir = _playerChest.forward;
        Debug.DrawRay(_playerChest.position, chestFowardDir * viewDistance, Color.yellow);


        // 여기서 부터 넣기

        Vector3 rotationDir = _viewNomalDir;
        rotationDir.y = 0;
        if (rotationDir == Vector3.zero) return;

        _yRotation = Quaternion.Lerp(_yRotation, Quaternion.LookRotation(rotationDir, Vector3.up), Time.deltaTime * _rotateSpeed);

        _attackPoint.LookAt(_playerChest.position + _viewNomalDir * _distanceFromPlayer, Vector3.up);
        Debug.DrawRay(_attackPoint.position, _attackPoint.forward * viewDistance, Color.green);
    }

    private void FixedUpdate()
    {
        _rigidbody.MoveRotation(_yRotation.normalized);
    }

    private void LateUpdate()
    {
        _spineBone.LookAt(_playerChest.position + _viewNomalDir * _distanceFromPlayer, Vector3.up);
        _spineBone.Rotate(_chestOffset);
    }
}
