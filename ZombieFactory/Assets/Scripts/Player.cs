using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MoveComponent _moveComponent;
    ViewComponent _viewComponent;
    Vector3 _dir;

    [SerializeField] float _moveSpeed = 5f;

    private void Start()
    {
        _moveComponent = GetComponent<MoveComponent>();
        _viewComponent = GetComponent<ViewComponent>();

        _moveComponent.Initialize();
        _viewComponent.Initialize();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        _dir = new Vector3(x, 0, z);
        _viewComponent.ResetView();
    }

    private void FixedUpdate()
    {
        _viewComponent.RotateRigidbody();
        _moveComponent.Move(_dir.normalized, _moveSpeed);
    }

    private void LateUpdate()
    {
        _viewComponent.RotateBody();
        _viewComponent.ResetCamera();
    }
}
