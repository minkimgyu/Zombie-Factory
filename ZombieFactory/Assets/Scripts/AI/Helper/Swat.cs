using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AI.Swat
{
    public class Swat : BaseLife, IInjectPathfind
    {
        public enum State
        {
            FreeRole,
            BuildFormation
        }

        State _state;

        float _targetCaptureRadius = 5;
        float _moveRange = 8;
        float _moveSpeed = 5;
        float _stateChangeDuration = 3;

        float _retreatDistance = 8;

        float _stopDistance = 1f;
        float _gap = 0.5f;


        float _farDistance = 5f;
        float _closeDistance = 2f;

        Animator _animator;

        TPSViewComponent _viewComponent;
        TPSMoveComponent _moveComponent;
        PathSeeker _pathSeeker;

        MovementFSM _movementFSM;

        [SerializeField] Transform _rig;
        [SerializeField] SightComponent _sightComponent;

        BaseFactory _ragdollFactory;

        Vector3 _offset;

        public void ResetPoints(Vector3 offset)
        {
            _offset = offset;
        }

        public override void ResetData(SwatData data, BaseFactory effectFactory, BaseFactory ragdollFactory)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            _animator = GetComponentInChildren<Animator>();

            _myType = IIdentifiable.Type.Zombie;

            Func<Vector3, Vector3, List<Vector3>> FindPath = FindObjectOfType<GroundPathfinder>().FindPath;

            _pathSeeker = GetComponent<PathSeeker>();
            _pathSeeker.Initialize(FindPath, false);

            _sightComponent.SetUp(5, 90, new List<IIdentifiable.Type> { IIdentifiable.Type.Human });
            _sightComponent.Resize(_targetCaptureRadius);

            _viewComponent = GetComponent<TPSViewComponent>();
            _viewComponent.Initialize(70);

            _moveComponent = GetComponent<TPSMoveComponent>();
            _moveComponent.Initialize();

            _movementFSM = new MovementFSM();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>
            {
                { 
                    State.FreeRole, 
                    new FreeRoleState
                    (
                        _movementFSM,
                        _moveSpeed,
                        _stateChangeDuration,
                        _retreatDistance,
                        _gap,

                        _farDistance,
                        _closeDistance,

                        _moveRange,
                        _viewComponent,
                        _moveComponent,
                        _animator,
                        transform,
                        _sightComponent,
                        _pathSeeker
                    ) 
                },
                { 
                    State.BuildFormation, 
                    new BuildFormationState
                    (
                        _movementFSM
                    ) 
                },
            };
            _movementFSM.Initialize(states);
            _movementFSM.SetState(State.FreeRole);
        }

        private void Update()
        {
            _movementFSM.OnUpdate();
        }

        private void FixedUpdate()
        {
            _movementFSM.OnFixedUpdate();
        }

        public override void ResetData(SwatData data, BaseFactory effectFactory)
        {
            _effectFactory = effectFactory;
        }

        public override void OnDieRequested()
        {
            Ragdoll ragdoll = _ragdollFactory.Create(Name.Rook, transform.position, transform.rotation);
            ragdoll.Activate(_rig);

            Destroy(gameObject);
        }

        public void AddPathfind(Func<Vector3, Vector3, List<Vector3>> FindPath)
        {
            _pathSeeker.Initialize(FindPath, false);
        }
    }
}