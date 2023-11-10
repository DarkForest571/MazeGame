using MazeGame.Core.AI;
using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Zombie : Entity, IAIControlable
    {
        private DefaultIdleState _idleState;
        private DefaultWanderingState _wanderingState;
        private ZombieAttackPreporationState _attackPreporationState;
        private DefaultAttackState _attackState;
        private DefaultFollowState _followState;

        private AIState _AIState;

        private Projectile _attackProjectile;
        private readonly int _viewDistanse;

        public Zombie(char zombieImage,
                      Projectile attackProjectile,
                      int health = 150,
                      float moveSpeed = 0.25f,
                      int viewDistance = 10,
                      Vector2 position = default) : base(zombieImage,
                                                         health,
                                                         moveSpeed,
                                                         position)
        {
            _idleState = new DefaultIdleState(1, 3);
            _wanderingState = new DefaultWanderingState();
            _attackPreporationState = new ZombieAttackPreporationState();
            _attackState = new DefaultAttackState(1);
            _followState = new DefaultFollowState();

            _idleState.SetNextStates(_wanderingState, _attackPreporationState);
            _wanderingState.SetNextStates(_idleState, _attackPreporationState);
            _attackPreporationState.SetNextStates(_followState, _attackState);
            _attackState.SetNextStates(_attackPreporationState, _followState);
            _followState.SetNextStates(_attackPreporationState, _idleState);

            _AIState = _idleState;

            _attackProjectile = attackProjectile;
            _viewDistanse = viewDistance;
        }

        public int ViewDistance => _viewDistanse;

        public override Zombie Clone() => new Zombie(Image, _attackProjectile, Health, MoveSpeed, _viewDistanse, Position);

        public void HandleAIState(World world, Vector2 playerPosition, bool canSeePlayer, int framesPerSecond)
        {
            AIState newState = _AIState.HandleAIState(world, Position, playerPosition, canSeePlayer, framesPerSecond);
            if (newState != null)
                _AIState = newState;
        }

        public Projectile GetAttack()
        {
            return _attackProjectile.Clone();
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
                        Projectile projectile = GetAttack();
                        projectile.Position = Position + state.AttackDirection;
                        projectile.Init(this, state.AttackDirection, framesPerSecond);
                        world.AddProjectile(projectile);
                    }
                    break;
            }
        }
    }
}
