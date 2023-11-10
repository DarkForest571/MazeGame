using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
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
}
