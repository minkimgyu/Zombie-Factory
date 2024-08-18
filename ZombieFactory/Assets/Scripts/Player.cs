using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MoveComponent _moveComponent;
    ViewComponent _viewComponent;

    private void Start()
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _viewComponent = GetComponent<ViewComponent>();
        _viewComponent.Initialize();
    }

    public void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _moveComponent.Move(input, 3);
        _viewComponent.ResetView();
    }

    private void FixedUpdate()
    {
        _viewComponent.Rotation();
    }

    private void LateUpdate()
    {
        _viewComponent.ResetCamera();
    }
}
