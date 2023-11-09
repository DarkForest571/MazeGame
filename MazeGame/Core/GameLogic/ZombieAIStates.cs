using MazeGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Core.GameLogic
{
    class ZombieIdleState : IdleState
    {
        private WanderingState _nextWanderingState;
        private AttackPreporationState _nextPreporationState;

        public ZombieIdleState(float idleSecondsFrom,
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
            throw new NotImplementedException();
        }

        
    }

    class ZombieWanderingState : WanderingState
    {
        private IdleState _nextIdleState;
        private AttackPreporationState _nextPreporationState;

        public ZombieWanderingState() { }

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }

    class ZombieAttackState : AttackState
    {
        private AttackPreporationState _nextPreporationState;
        private FollowState _nextFollowState;

        public ZombieAttackState(float secondsRerAttack) : base(secondsRerAttack)
        { }

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
            throw new NotImplementedException();
        }
    }

    class ZombieFollowState : FollowState
    {
        private AttackPreporationState _nextPreporationState;
        private IdleState _nextIdleState;

        public ZombieFollowState() { }

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
            throw new NotImplementedException();
        }
    }
}
