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

        float _destoryDelay;
        float _stageChangeDuration;

        float _moveSpeed;
        float _viewSpeed;

        float _stopDistance;
        float _gap;

        float _targetCaptureRadius;

        float _noiseCaptureRadius;
        int _maxNoiseQueueSize;

        float _attackRadius;
        float _attackDamage;

        float _attackPreDelay;
        float _attackAfterDelay;

        float _moveRange;

        Animator _animator;

        BaseViewComponent _viewComponent;
        BaseMoveComponent _moveComponent;
        PathSeeker _pathSeeker;

        ZombieFSM _zombieFSM;

        Queue<Vector3> _noiseQueue;
        List<ITarget> _targetList;

        BaseFactory _ragdollFactory;

        [SerializeField] Transform _rig;
        [SerializeField] Name _ragdoolName;
        [SerializeField] SightComponent _sightComponent;
        [SerializeField] TargetCaptureComponent _noiseCaptureComponent;

        [SerializeField] Transform _sightPoint; // 시아 포인트, 공격 포인트

        void OnNoiseEnter(ITarget target)
        {
            if (_noiseQueue.Count >= _maxNoiseQueueSize)
            {
                _noiseQueue.Dequeue(); // 하나 뺴준다.
            }

            bool isOpponent = target.IsOpponent(new List<IIdentifiable.Type> { IIdentifiable.Type.Sound });
            if (isOpponent == false) return;

            _noiseQueue.Enqueue(target.ReturnTargetPoint().position); // 그리고 추가해준다.
            _zombieFSM.OnNoiseEnter();
        }

        Action OnDie;

        public override void AddObserverEvent(Action OnDie)
        {
            this.OnDie = OnDie;
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

        protected override void OnDieRequested()
        {
            Ragdoll ragdoll = _ragdollFactory.Create(_ragdoolName, transform.position, transform.rotation);
            ragdoll.Activate(_rig);

            OnDie?.Invoke();
            Destroy(gameObject);
        }

        void ResetAnimator(Vector3 dir)
        {
            if (dir == Vector3.zero)
            {
                _animator.SetBool("Run", false);
            }
            else
            {
                _animator.SetBool("Run", true);
            }
        }

        public override void InitializeFSM()
        {
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
                    _sightPoint,
                    transform,
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

        public override void Initialize()
        {
            base.Initialize();

            _animator = GetComponentInChildren<Animator>();

            _myType = IIdentifiable.Type.Zombie;
            _noiseQueue = new Queue<Vector3>();
            _targetList = new List<ITarget>();

            _pathSeeker = GetComponent<PathSeeker>();
            _pathSeeker.Initialize();

            _noiseCaptureComponent.Initialize(OnNoiseEnter);
            _noiseCaptureComponent.Resize(_noiseCaptureRadius);
            
            _sightComponent.SetUp(_targetCaptureRadius, 270, new List<IIdentifiable.Type> { IIdentifiable.Type.Human }, _sightPoint);

            Rigidbody rigidbody = GetComponent<Rigidbody>();

            _viewComponent = GetComponent<BaseViewComponent>();
            _viewComponent.Initialize(70, rigidbody);

            _moveComponent = GetComponent<BaseMoveComponent>();
            _moveComponent.Initialize(rigidbody, ResetAnimator);
        }

        // Update is called once per frame
        void Update()
        {
            _moveComponent.CheckIsOnSlope();
            _zombieFSM.OnUpdate();
        }

        private void FixedUpdate()
        {
            _zombieFSM.OnFixedUpdate();
        }

        public override Transform ReturnSightPoint()
        {
            return _sightPoint;
        }
    }
}