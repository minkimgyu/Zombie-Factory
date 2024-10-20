using AI.Swat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Swat.Battle
{
    abstract public class BaseBattleState : BaseState<Swat.BattleState>
    {
        public BaseBattleState(FSM<Swat.BattleState> fsm) : base(fsm)
        {
        }

        public override void OnStateEnter() { }
        public override void OnStateExit() { }
        public override void OnStateUpdate() { }
    }

    public class BattleFSM : FSM<Swat.BattleState>
    {
    }
}