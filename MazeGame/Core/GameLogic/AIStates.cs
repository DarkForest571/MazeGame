using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface AIState
    {
        AIState? HandleAIState(World world,
                               Vector2 entityPosition,
                               Vector2 playerPosition,
                               bool canSeePlayer,
                               int framesPerSecond);

        void Update();
    }

    abstract class IdleState : AIState
    {
        protected int _idleFramesTimer;
        protected readonly float _idleMinSeconds;
        protected readonly float _idleMaxSeconds;

        protected IdleState(float idleSecondsFrom, float idleSecondsTo)
        {
            _idleFramesTimer = 0;
            _idleMinSeconds = idleSecondsFrom;
            _idleMaxSeconds = idleSecondsTo;
        }

        public abstract AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond);

        public void Update()
        {
            if (_idleFramesTimer > 0)
                _idleFramesTimer--;
        }

        public virtual void InitState(int framesPerSecond)
        {
            int min = (int)(_idleMinSeconds * framesPerSecond);
            int max = (int)(_idleMaxSeconds * framesPerSecond);
            _idleFramesTimer = Random.Shared.Next(min, max);
        }
    }

    abstract class WanderingState : AIState
    {
        protected Vector2 _targetPosition;

        protected WanderingState() { }

        public abstract AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond);

        public void Update() { }

        public virtual void InitState(World world, Vector2 entityPosition)
        {
            List<Direction> availableDirection = world.GetNeighborsByCondition(entityPosition, (tile) => tile.IsPassable);
            if (availableDirection.Count > 0)
            {
                int choise = Random.Shared.Next(availableDirection.Count);
                _targetPosition = entityPosition + availableDirection[choise];
            }
            else
            {
                _targetPosition = entityPosition;
            }
        }
    }

    abstract class AttackPreporationState : AIState
    {
        protected Vector2 _attackPosition;
        protected Vector2 _playerPosition;

        protected AttackPreporationState() { }

        public abstract AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond);

        public void Update() { }

        public virtual void InitState(World world, Vector2 entityPosition, Vector2 playerPosition)
        {
            _attackPosition = playerPosition + Vector2.GetDirection(playerPosition, entityPosition);
            _playerPosition = playerPosition;
        }
    }

    abstract class AttackState : AIState
    {
        protected float _secondsPerAttck;
        protected int _attackTimer;

        protected Vector2 _playerPosition;

        protected AttackState(float secondsRerAttack)
        {
            _attackTimer = 0;
            _secondsPerAttck = secondsRerAttack;
        }

        public abstract AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond);

        public void Update() { }

        public virtual void InitState(Vector2 playerPosition, int framesPerSecond)
        {
            _playerPosition = playerPosition;
            _attackTimer = (int)(_secondsPerAttck * framesPerSecond);
        }
    }

    abstract class FollowState : AIState
    {
        protected Vector2 _lastPlayerPosition;

        protected FollowState() { }

        public abstract AIState? HandleAIState(World world,
                                               Vector2 entityPosition,
                                               Vector2 playerPosition,
                                               bool canSeePlayer,
                                               int framesPerSecond);

        public void Update() { }

        public void InitState(Vector2 lastPlayerPosition)
        {
            _lastPlayerPosition = lastPlayerPosition;
        }
    }
}
