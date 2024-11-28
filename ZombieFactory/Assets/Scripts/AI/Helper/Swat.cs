using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using AI.Swat.Movement;
using AI.Swat.Battle;

namespace AI.Swat
{
    public class FormationData
    {
        ITarget _target;
        public ITarget Target { get { return _target; } set { _target = value; } }

        Vector3 _offset;
        public Vector3 Offset { get { return _offset; } set { _offset = value; } }
    }

    public class Swat : BaseLife, IHelper, IWeaponEquipable
    {
        public enum MovementState
        {
            FreeRole,
            BuildFormation
        }

        public enum BattleState
        {
            Idle,
            Attack
        }

        FormationData _formationData;
        MovementState _state;

        float _moveRange;
        float _moveSpeed;
        float _stateChangeDuration;

        float _retreatDistance;

        float _stopDistance;
        float _gap;


        float _farDistance;
        float _closeDistance;

        float _attackDuration;
        float _attackDelay;

        float _weaponThrowPower;

        float _captureRadius;
        float _captureAngle;

        Animator _animator;

        WeaponController _weaponController;

        MovementFSM _movementFSM;
        BattleFSM _battleFSM;

        [SerializeField] Name _ragdollName;
        [SerializeField] Transform _rig;
        [SerializeField] SightComponent _sightComponent;

        [SerializeField] Transform _attackPoint;
        [SerializeField] Transform _sightPoint;

        FreeRoleState _freeRoleState;
        BaseFactory _ragdollFactory;

        TPSMoveComponent _moveComponent;
        TPSViewComponent _viewComponent;
        PathSeeker _pathSeeker;

        public override void ResetData(SwatData data, BaseFactory effectFactory, BaseFactory ragdollFactory)
        {
            _maxHp = data.maxHp;
            _hp = _maxHp;

            _moveRange = data.moveRange;
            _moveSpeed = data.moveSpeed;

            _stateChangeDuration = data.stateChangeDuration;
            _retreatDistance = data.retreatDistance;

            _stopDistance = data.stopDistance;
            _gap = data.gap;

            _farDistance = data.farDistance;
            _closeDistance = data.closeDistance;

            _attackDuration = data.attackDuration;
            _attackDelay = data.attackDelay;
            _weaponThrowPower = data.weaponThrowPower;

            _captureRadius = data.captureRadius;
            _captureAngle = data.captureAngle;

            _effectFactory = effectFactory;
            _ragdollFactory = ragdollFactory;
        }

        public void AddWeapon(BaseWeapon weapon)
        {
            _weaponController.OnWeaponReceived(weapon);
        }

        Action<IHelper> RemoveHelper;

        public void OnAddHelper(ITarget player, Action<IHelper> RemoveHelper)
        {
            _formationData = new FormationData();
            _formationData.Target = player;

            this.RemoveHelper = RemoveHelper;
        }

        public void RestOffset(Vector3 offset)
        {
            _formationData.Offset = offset;
        }

        public void GetAmmoPack(int ammoCount)
        {
            _weaponController.RefillAmmo(ammoCount);
        }

        public void GetAidPack(float healPoint)
        {
            GetHeal(healPoint);
        }

        Vector3 _storedDir;

        void ResetAnimator(Vector3 dir)
        {
            _storedDir = Vector3.Lerp(_storedDir, dir, Time.deltaTime * _moveSpeed);
            _animator.SetFloat("X", _storedDir.x);
            _animator.SetFloat("Z", _storedDir.z);
        }

        public override void InitializeFSM()
        {
            _battleFSM = new BattleFSM();
            Dictionary<BattleState, BaseState<BattleState>> battleStates = new Dictionary<BattleState, BaseState<BattleState>>
            {
                {
                    BattleState.Idle,
                    new Battle.IdleState
                    (
                        _battleFSM,
                        _weaponController,
                        _sightComponent

                    )
                },
                {
                    BattleState.Attack,
                    new AttackState
                    (
                        _battleFSM,
                        _weaponController,
                        _sightComponent,
                        _attackDuration,
                        _attackDelay
                    )
                },
            };

            _battleFSM.Initialize(battleStates);
            _battleFSM.SetState(BattleState.Idle);

            _movementFSM = new MovementFSM();
            Dictionary<MovementState, BaseState<MovementState>> movementStates = new Dictionary<MovementState, BaseState<MovementState>>
            {
                {
                    MovementState.FreeRole,
                    new FreeRoleState
                    (
                        _movementFSM,
                        _moveSpeed,
                        _stateChangeDuration,
                        _moveRange,
                        _retreatDistance,
                        _gap,

                        _farDistance,
                        _closeDistance,

                        _formationData,

                        _viewComponent,
                        _moveComponent,
                        transform,
                        _sightComponent,
                        _pathSeeker
                    )
                },
                {
                    MovementState.BuildFormation,
                    new BuildFormationState
                    (
                        _movementFSM,
                        _moveSpeed,
                        _captureRadius,
                        _gap,

                        _formationData,

                        _viewComponent,
                        _moveComponent,
                        transform,
                        _sightComponent,
                        _pathSeeker
                    )
                },
            };

            _movementFSM.Initialize(movementStates);
            _movementFSM.SetState(MovementState.FreeRole);
        }

        public override void Initialize()
        {
            base.Initialize();

            _weaponController = GetComponent<WeaponController>();
            _weaponController.Initialize(_attackPoint);

            _animator = GetComponentInChildren<Animator>();

            _myType = IIdentifiable.Type.Human;

            _pathSeeker = GetComponent<PathSeeker>();
            _pathSeeker.Initialize();

            _sightComponent.SetUp(_captureRadius, _captureAngle, new List<IIdentifiable.Type> { IIdentifiable.Type.Zombie }, _sightPoint);
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            _viewComponent = GetComponent<TPSViewComponent>();
            _viewComponent.Initialize(70, rigidbody, _attackPoint);

            _moveComponent = GetComponent<TPSMoveComponent>();
            _moveComponent.Initialize(rigidbody, ResetAnimator);
        }

        public void ChangeState(MovementState state)
        {
            _movementFSM.SetState(state); // 공격 스테이트도 추가

        }

        private void Update()
        {
            _moveComponent.CheckIsOnSlope();

            _weaponController.OnUpdate();
            _battleFSM.OnUpdate();
            _movementFSM.OnUpdate();
        }

        private void LateUpdate()
        {
            _viewComponent.RotateSpineBone();
        }

        private void FixedUpdate()
        {
            _battleFSM.OnFixedUpdate();
            _movementFSM.OnFixedUpdate();
        }

        protected override void OnDieRequested()
        {
            Ragdoll ragdoll = _ragdollFactory.Create(_ragdollName, transform.position, transform.rotation);
            ragdoll.Activate(_rig);

            RemoveHelper?.Invoke(this);
            Destroy(gameObject);
        }

        public void TeleportTo(Vector3 pos)
        {
            ResetPosition(pos);
        }

        public override Transform ReturnSightPoint()
        {
            return _sightPoint;
        }
    }
}