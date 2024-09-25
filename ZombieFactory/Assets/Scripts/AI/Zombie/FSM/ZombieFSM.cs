using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    abstract public class BaseZombieState : BaseState<Zombie.State>
    {
        public BaseZombieState(FSM<Zombie.State> fsm) : base(fsm)
        {
        }

        public override void OnStateEnter() { }
        public override void OnStateExit() { }
        public override void OnStateUpdate() { }

        public override void OnNoiseEnter() { }
        public override void OnTargetEnter() { }
        public override void OnTargetExit() { }
    }


    public class ZombieFSM : FSM<Zombie.State>
    {
        public void OnNoiseEnter() => _currentState.OnNoiseEnter();
        public void OnTargetEnter() => _currentState.OnTargetEnter();
        public void OnTargetExit() => _currentState.OnTargetExit();


        public void OnStateFixedUpdate() => _currentState.OnStateFixedUpdate();
        public void OnCollisionEnter(Collision collision) => _currentState.OnCollisionEnter(collision);
    }
}