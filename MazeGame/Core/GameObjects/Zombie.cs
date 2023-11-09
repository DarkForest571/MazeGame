using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Zombie : Entity, IAIControlable
    {
        private readonly Projectile _attackProjectile;
        private Direction _attackDirection;

        private AIState _AIState;

        public Zombie(char zombieImage,
                      Projectile attackProjectile,
                      Vector2 position = default,
                      int health = 150,
                      float moveSpeed = 0.25f) : base(zombieImage,
                                                      position,
                                                      health,
                                                      moveSpeed)
        {
            _attackProjectile = attackProjectile;
            _attackDirection = Direction.Right;

            ZombieIdleState idleState = new ZombieIdleState(1, 3);
            ZombieWanderingState wanderingState = new ZombieWanderingState();
            ZombieAttackPreporationState attackPreporationState = new ZombieAttackPreporationState();
            ZombieAttackState attackState = new ZombieAttackState(1);
            ZombieFollowState followState = new ZombieFollowState();

            idleState.SetNextStates(wanderingState, attackPreporationState);
            wanderingState.SetNextStates(idleState, attackPreporationState);
            attackPreporationState.SetNextStates(followState, attackState);
            attackState.SetNextStates(attackPreporationState, followState);
            followState.SetNextStates(attackPreporationState, idleState);

            _AIState = idleState;
        }

        public override Zombie Clone() => new Zombie(Image, _attackProjectile, Position, Health, MoveSpeed);

        public void HandleAIState(World world, Vector2 playerPosition, bool canSeePlayer, int framesPerSecond)
        {
            AIState newState = _AIState.HandleAIState(world, Position, playerPosition, canSeePlayer, framesPerSecond);
            if (newState != null)
                _AIState = newState;
        }

        public override Projectile? GetAttack()
        {
            _attackProjectile.Position = Position + _attackDirection;
            return _attackProjectile.Clone();
        }
    }
}
