using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree.Nodes
{
    public class WaitForNextAttack : Node
    {
        Timer _delayTimer; // 공격 전 딜레이 적용
        Timer _animationDelayTimer; // 애니메이션 플레이 후 딜레이 적용

        float _preDelayDuration;
        float _afterDelayDuration;
        Action PlayAttackAnimation;

        public WaitForNextAttack(float preDelayDuration, float afterDelayDuration, Action PlayAttackAnimation)
        {
            _delayTimer = new Timer();
            _animationDelayTimer = new Timer();

            _preDelayDuration = preDelayDuration;
            _afterDelayDuration = afterDelayDuration;
            this.PlayAttackAnimation = PlayAttackAnimation;
        }

        public override NodeState Evaluate()
        {
            switch (_delayTimer.CurrentState)
            {
                case Timer.State.Ready:
                    Debug.Log("StartAttack");
                    _delayTimer.Start(_afterDelayDuration);
                    return NodeState.FAILURE;

                case Timer.State.Running:
                    return NodeState.RUNNING;

                case Timer.State.Finish:
                    if (_animationDelayTimer.CurrentState != Timer.State.Ready) break;

                    Debug.Log("PlayAttackAnimation");
                    PlayAttackAnimation?.Invoke();
                    _animationDelayTimer.Start(_preDelayDuration);
                    break;
            }

            switch (_animationDelayTimer.CurrentState)
            {
                case Timer.State.Ready:
                    return NodeState.FAILURE;
                case Timer.State.Running:
                    return NodeState.RUNNING;
                case Timer.State.Finish:
                    _delayTimer.Reset();
                    _animationDelayTimer.Reset();
                    return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }

        public override void OnDisableRequested()
        {
            _delayTimer.Reset();
        }
    }
}