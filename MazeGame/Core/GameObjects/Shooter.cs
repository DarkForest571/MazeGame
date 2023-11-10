using MazeGame.Core.AI;
using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Shooter : Entity, IAIControlable
    {
        private DefaultIdleState _idleState;
        private DefaultWanderingState _wanderingState;
        private ShooterAttackPreporationState _attackPreporationState;
        private DefaultAttackState _attackState;
        private DefaultFollowState _followState;

        private AIState _AIState;

        Projectile _horizontalAttackProjectile;
        Projectile _verticalAttackProjectile;
        private readonly int _viewDistanse;

        public Shooter(char image,
                      Projectile horizontalAttackProjectile,
                      Projectile verticalAttackProjectile,
                      int health = 75,
                      float moveSpeed = 1.5f,
                      int viewDistance = 10,
                      Vector2 position = default) : base(image,
                                                         health,
                                                         moveSpeed,
                                                         position)
        {
            _idleState = new DefaultIdleState(1, 3);
            _wanderingState = new DefaultWanderingState();
            _attackPreporationState = new ShooterAttackPreporationState(3);
            _attackState = new DefaultAttackState(1);
            _followState = new DefaultFollowState();

            _idleState.SetNextStates(_wanderingState, _attackPreporationState);
            _wanderingState.SetNextStates(_idleState, _attackPreporationState);
            _attackPreporationState.SetNextStates(_followState, _attackState);
            _attackState.SetNextStates(_attackPreporationState, _followState);
            _followState.SetNextStates(_attackPreporationState, _idleState);

            _AIState = _idleState;

            _horizontalAttackProjectile = horizontalAttackProjectile;
            _verticalAttackProjectile = verticalAttackProjectile;
            _viewDistanse = viewDistance;
        }

        public int ViewDistance => _viewDistanse;

        public override Shooter Clone() => new Shooter(Image,
                                                       _horizontalAttackProjectile,
                                                       _verticalAttackProjectile,
                                                       Health,
                                                       MoveSpeed,
                                                       _viewDistanse,
                                                       Position);


        public void HandleAIState(World world, Vector2 playerPosition, bool canSeePlayer, int framesPerSecond)
        {
            AIState newState = _AIState.HandleAIState(world, Position, playerPosition, canSeePlayer, framesPerSecond);
            if (newState != null)
                _AIState = newState;
        }

        public Projectile GetAttack(Direction direction)
        {
            if (direction == Direction.Right || direction == Direction.Left)
            {
                return _horizontalAttackProjectile.Clone();
            }
            else
            {
                return _verticalAttackProjectile.Clone();
            }
        }

        public void AIAction(World world, int framesPerSecond)
        {
            _AIState.Update();
            Direction direction;
            switch (_AIState)
            {
                case IdleState:
                    break;
                case MoveAIState state:
                    UpdateMoveTimer();
                    direction = Vector2.GetDirection(Position, state.TargetPosition);
                    MoveTo(direction, framesPerSecond);
                    break;
                case AttackState state:
                    if (state.ReadyForAttck)
                    {
                        Projectile projectile = GetAttack(state.AttackDirection);
                        projectile.Position = Position + state.AttackDirection;
                        projectile.Init(this, state.AttackDirection, framesPerSecond);
                        world.AddProjectile(projectile);
                    }
                    break;
            }
        }
    }
}
