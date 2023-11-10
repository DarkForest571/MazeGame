using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    class ShooterAttackPreporationState : AttackPreporationState
    {
        private FollowState _nextFollowState;
        private AttackState _nextAttackState;

        private int _attackDistance;

        public ShooterAttackPreporationState(int attackDistance)
        {
            _attackDistance = attackDistance;
        }

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

        public override void InitState(World world, Vector2 entityPosition, Vector2 playerPosition)
        {
            Vector2 deltaPosition = Vector2.FromDirection(Vector2.GetDirection(playerPosition, entityPosition));
            _attackPosition = playerPosition;
            for (int i = 0; i < _attackDistance; i++)
            {
                if (world[_attackPosition + deltaPosition].IsPassable)
                    _attackPosition += deltaPosition;
                else
                    break;
            }
            _lastPlayerPosition = playerPosition;
        }
    }
}
