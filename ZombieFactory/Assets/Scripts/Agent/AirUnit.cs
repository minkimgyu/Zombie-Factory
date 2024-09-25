using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseMoveComponent))]
public class AirUnit : MonoBehaviour
{
    [SerializeField] Transform _endTarget;
    [SerializeField] AirPathfinder _pathfinder;
    [SerializeField] float _moveSpeed;

    PathSeeker _pathSeeker;
    BaseMoveComponent _moveComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        _pathSeeker = GetComponent<PathSeeker>();
        _moveComponent = GetComponent<BaseMoveComponent>();

        //FindObjectOfType<pathfinder>

        //_pathSeeker.Initialize()
        //_moveComponent.Initialize(_pathfinder.FindPath, true, _moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //_moveComponent.Move(_endTarget.position);
    }
}
