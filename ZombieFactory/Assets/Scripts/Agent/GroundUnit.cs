using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundUnit : MonoBehaviour
{
    [SerializeField] Transform _endTarget;
    [SerializeField] GroundPathfinder _pathfinder;
    [SerializeField] float _moveSpeed;

    BaseViewComponent _viewComponent;
    BaseMoveComponent _moveComponent;
    PathSeeker _pathSeeker;

    // Start is called before the first frame update
    void Start()
    {
        _pathSeeker = GetComponent<PathSeeker>();
        _pathSeeker.Initialize(_pathfinder.FindPath, false);

        _viewComponent = GetComponent<BaseViewComponent>();
        _viewComponent.Initialize(70);

        _moveComponent = GetComponent<BaseMoveComponent>();
        _moveComponent.Initialize();
    }

    Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        direction = _pathSeeker.ReturnDirection(_endTarget.position);
        _viewComponent.View(direction);
    }

    private void FixedUpdate()
    {
        _moveComponent.Move(direction, _moveSpeed);
        _viewComponent.RotateRigidbody();
    }
}
