using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    ActionController _actionController;

    private void Start()
    {
        _actionController = GetComponent<ActionController>();
        _actionController.Initialize(5, 8, 3, 7, 10f, 0.76f, 1.54f, 0.76f / 2, 1.54f / 2, 50, new Vector2(150, 150));

        IInputable inputable = ServiceLocater.ReturnInputHandler();
        inputable.AddEvent(IInputable.Type.Jump, new KeyCommand(_actionController.OnHandleJump));
        inputable.AddEvent(IInputable.Type.Move, new MoveCommand(_actionController.OnHandleMove));

        inputable.AddEvent(IInputable.Type.CrouchStart, new KeyCommand(_actionController.OnHandleSit));
        inputable.AddEvent(IInputable.Type.CrouchEnd, new KeyCommand(_actionController.OnHandleStand));

        inputable.AddEvent(IInputable.Type.RunStart, new KeyCommand(_actionController.OnHandleRunStart));
        inputable.AddEvent(IInputable.Type.RunEnd, new KeyCommand(_actionController.OnHandleRunEnd));
        // 이런 식으로 추가
    }

    private void OnCollisionEnter(Collision collision)
    {
        _actionController.OnCollisionEnterRequested(collision);
    }

    private void Update()
    {
        _actionController.OnUpdate();
    }

    private void FixedUpdate()
    {
        _actionController.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        _actionController.OnLateUpdate();
    }
}
