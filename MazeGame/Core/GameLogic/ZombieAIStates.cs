using MazeGame.Core.GameObjects;
using MazeGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Core.GameLogic
{
    class DefaultIdleState : IdleState
    {
        private WanderingState _nextWanderingState;
        private AttackPreporationState _nextPreporationState;

        public DefaultIdleState(float idleSecondsFrom,
                               float idleSecondsTo) : base(idleSecondsFrom,
                                                           idleSecondsTo)
        { }

        public void SetNextStates(WanderingState wanderingState, AttackPreporationState attackPreporationState)
        {
            _nextWanderingState = wanderingState;
            _nextPreporationState = attackPreporationState;
        }

        public override AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond)
        {
            if (canSeePlayer)
            {
                _nextPreporationState.InitState(world, entityPosition, playerPosition);
                return _nextPreporationState;
            }

            if (_idleFramesTimer == 0)
            {
                _nextWanderingState.InitState(world, entityPosition);
                return _nextWanderingState;
            }
            return null;
        }


    }

    class DefaultWanderingState : WanderingState
    {
        private IdleState _nextIdleState;
        private AttackPreporationState _nextPreporationState;

        public DefaultWanderingState() { }

        public void SetNextStates(IdleState idleState, AttackPreporationState attackPreporationState)
        {
            _nextIdleState = idleState;
            _nextPreporationState = attackPreporationState;
        }

        public override AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond)
        {
            if (canSeePlayer)
            {
                _nextPreporationState.InitState(world, entityPosition, playerPosition);
                return _nextPreporationState;
            }

            if (entityPosition == _targetPosition)
            {
                _nextIdleState.InitState(framesPerSecond);
                return _nextIdleState;
            }
            return null;
        }
    }

    class ZombieAttackPreporationState : AttackPreporationState
    {
        private FollowState _nextFollowState;
        private AttackState _nextAttackState;

        public ZombieAttackPreporationState() { }

        public void SetNextStates(FollowState followState, AttackState attackState)
        {
            _nextFollowState = followState;
            _nextAttackState = attackState;
        }

        public override AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond)
        {
            if (!canSeePlayer)
            {
                _nextFollowState.InitState(_lastPlayerPosition);
                return _nextFollowState;
            }

            if (playerPosition != _lastPlayerPosition)
            {
                InitState(world, entityPosition, playerPosition);
                return this;
            }

            if (entityPosition == _attackPosition)
            {
                _nextAttackState.InitState(entityPosition, playerPosition, framesPerSecond);
                return _nextAttackState;
            }
            return null;
        }
    }

    class ZombieAttackState : AttackState
    {
        private AttackPreporationState _nextPreporationState;
        private FollowState _nextFollowState;

        public ZombieAttackState(float secondsRerAttack) : base(secondsRerAttack) { }

        public void SetNextStates(AttackPreporationState attackPreporationState, FollowState followState)
        {
            _nextPreporationState = attackPreporationState;
            _nextFollowState = followState;
        }

        public override AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond)
        {
            if (!canSeePlayer)
            {
                _nextFollowState.InitState(_lastPlayerPosition);
                return _nextFollowState;
            }

            if (playerPosition != _lastPlayerPosition)
            {
                _nextPreporationState.InitState(world, entityPosition, playerPosition);
                return _nextPreporationState;
            }

            if(_attackTimer == 0)
            {
                InitState(entityPosition, playerPosition, framesPerSecond);
                return this;
            }
            return null;
        }

    }

    class DefaultFollowState : FollowState
    {
        private AttackPreporationState _nextPreporationState;
        private IdleState _nextIdleState;

        public DefaultFollowState() { }

        public void SetNextStates(AttackPreporationState attackPreporationState, IdleState idleState)
        {
            _nextPreporationState = attackPreporationState;
            _nextIdleState = idleState;
        }

        public override AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond)
        {
            if (canSeePlayer)
            {
                _nextPreporationState.InitState(world, entityPosition, playerPosition);
                return _nextPreporationState;
            }

            if (entityPosition == _lastPlayerPosition)
            {
                _nextIdleState.InitState(framesPerSecond);
                return _nextIdleState;
            }
            return null;
        }
    }
}
