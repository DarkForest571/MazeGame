using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Player : Entity
    {
        private readonly Projectile _attackProjectile;
        private Direction _attackDirection;

        public Player(char playerImage,
                      Projectile attackProjectile,
                       Vector2 position = default,
                      int health = 100,
                      float moveSpeed = 1.0f) : base(playerImage,
                                                     position,
                                                     health,
                                                     moveSpeed)
        {
            _attackProjectile = attackProjectile;
            _attackDirection = Direction.Right;
        }

        public override Player Clone() => new Player(Image, _attackProjectile, Position, Health, MoveSpeed);

        public override Projectile? GetAttack()
        {
            if (AttackTimer == 0)
            {
                _attackProjectile.Position = Position + _attackDirection;
                return _attackProjectile.Clone();
            }
            else
                return null;
        }
    }
}
