using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Swat
{
    abstract public class BaseMovementState : BaseState<Swat.State>
    {
        public BaseMovementState(FSM<Swat.State> fsm) : base(fsm)
        {
        }

        public override void OnStateEnter() { }
        public override void OnStateExit() { }
        public override void OnStateUpdate() { }
    }

    abstract public class BaseFreeRoleState : BaseState<FreeRoleState.State>
    {
        public BaseFreeRoleState(FSM<FreeRoleState.State> fsm) : base(fsm)
        {
        }

        public override void OnStateEnter() { }
        public override void OnStateExit() { }
        public override void OnStateUpdate() { }
    }

    public class FreeRoleFSM : FSM<FreeRoleState.State>
    {
    }

    public class MovementFSM : FSM<Swat.State>
    {
    }
}