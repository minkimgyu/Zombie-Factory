using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Zombie
{
    public class Zombie : BaseLife
    {
        public enum State
        {
            Idle,
            NoiseTracking,
            TargetFollowing
        }

        float _destoryDelay = 5;
        float _stageChangeDuration = 3;

        float _moveSpeed = 5;
        float _viewSpeed = 5;

        float _stopDistance = 1f;
        float _gap = 0.5f;

        float _targetCaptureRadius = 5;

        float _noiseCaptureRadius = 11;
        int _maxNoiseQueueSize = 3;

        float _attackRadius = 1.5f;
        float _attackDamage = 30;

        float _attackPreDelay = 0.5f;
        float _attackAfterDelay = 3;

        float _moveRange = 5;

        Animator _animator;

        TPSViewComponent _viewComponent;
        TPSMoveComponent _moveComponent;
        PathSeeker _pathSeeker;

        ZombieFSM _zombieFSM;
        [SerializeField] Transform _raycastPoint;

        Queue<Vector3> _noiseQueue;
        List<ITarget> _targetList;

        BaseFactory _ragdollFactory;

        [SerializeField] Transform _rig;

        [SerializeField] SightComponent _sightComponent;
        [SerializeField] TargetCaptureComponent _noiseCaptureComponent;

        void OnNoiseEnter(ITarget target)
        {
            if (_noiseQueue.Count == _maxNoiseQueueSize) return;

            bool isOpponent = target.IsOpponent(new List<IIdentifiable.Type> { IIdentifiable.Type.Sound });
            if (isOpponent == false) return;

            _noiseQueue.Enqueue(target.ReturnPosition());
            _zombieFSM.OnNoiseEnter();
        }

        public override void ResetData(ZombieData data, BaseFactory effectFactory, BaseFactory ragdollFactory) 
        {
            _maxHp = data.maxHp;

            _destoryDelay = data.destoryDelay;
            _stageChangeDuration = data.stageChangeDuration;

            _moveSpeed = data.moveSpeed;
            _viewSpeed = data.viewSpeed;

            _stopDistance = data.stopDistance;
            _gap = data.gap;

            _targetCaptureRadius = data.targetCaptureRadius;

            _noiseCaptureRadius = data.noiseCaptureRadius;
            _maxNoiseQueueSize = data.maxNoiseQueueSize;

            _attackRadius = data.attackRadius;
            _attackDamage = data.attackDamage;

            _attackPreDelay = data.attackPreDelay;
            _attackAfterDelay = data.attackAfterDelay;

            _effectFactory = effectFactory;
            _ragdollFactory = ragdollFactory;
        }

        public override void OnDieRequested()
        {
            Ragdoll ragdoll = _ragdollFactory.Create(Name.PoliceZombie, transform.position, transform.rotation);
            ragdoll.Activate(_rig);

            Destroy(gameObject);
        }

        public override void Initialize()
        {
            base.Initialize();

            _animator = GetComponentInChildren<Animator>();

            _myType = IIdentifiable.Type.Zombie;
            _noiseQueue = new Queue<Vector3>();
            _targetList = new List<ITarget>();

            _pathSeeker = GetComponent<PathSeeker>();

            _noiseCaptureComponent.Initialize(OnNoiseEnter);
            _noiseCaptureComponent.Resize(_noiseCaptureRadius);
            
            _sightComponent.SetUp(5, 90, new List<IIdentifiable.Type> { IIdentifiable.Type.Human });
            _sightComponent.Resize(_targetCaptureRadius);

            _viewComponent = GetComponent<TPSViewComponent>();
            _viewComponent.Initialize(70);

            _moveComponent = GetComponent<TPSMoveComponent>();
            _moveComponent.Initialize();

            _zombieFSM = new ZombieFSM();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>
            {
                {
                    State.Idle, new IdleState(
                    _zombieFSM,
                    _moveSpeed,
                    _stageChangeDuration,
                    _moveRange,
                    _viewComponent,
                    _moveComponent,
                    _animator,
                    _pathSeeker,
                    transform,
                    _sightComponent) },
                {
                    State.NoiseTracking, new NoiseTrackingState(
                    _zombieFSM,
                    _moveSpeed,
                    _viewComponent,
                    _moveComponent,
                    _sightComponent,
                    _animator,
                    _pathSeeker,
                    _noiseQueue)
                },
                {
                    State.TargetFollowing, new TargetFollowingState(
                    _zombieFSM,
                    _moveSpeed,
                    _stopDistance,
                    _gap,
                    _attackRadius,
                    _attackDamage,
                    _attackPreDelay,
                    _attackAfterDelay,
                    transform,
                    _raycastPoint,
                    _viewComponent,
                    _moveComponent,
                    _pathSeeker,
                    _sightComponent,
                    _animator)
                },
            };
            _zombieFSM.Initialize(states);
            _zombieFSM.SetState(State.Idle);
        }

        // Update is called once per frame
        void Update()
        {
            _zombieFSM.OnUpdate();
        }

        private void FixedUpdate()
        {
            _zombieFSM.OnFixedUpdate();
        }
    }
}