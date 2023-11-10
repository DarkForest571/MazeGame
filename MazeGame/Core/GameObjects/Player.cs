using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Player : Entity
    {
        private readonly Projectile _attackProjectile;
        private Direction _attackDirection;
        private int _attackTimer;

        public Player(char playerImage,
                      Projectile attackProjectile,
                      int health = 100,
                      float moveSpeed = 1.0f,
                       Vector2 position = default) : base(playerImage,
                                                     health,
                                                     moveSpeed,
                                                     position)
        {
            _attackProjectile = attackProjectile;
            _attackDirection = Direction.Right;
            _attackTimer = 0;
        }

        public override Player Clone() => new Player(Image, _attackProjectile, Health, MoveSpeed, Position);

        public override Projectile? GetAttack()
        {
            if (_attackTimer == 0)
            {
                _attackProjectile.Position = Position + _attackDirection;
                return _attackProjectile.Clone();
            }
            else
                return null;
        }
    }
}
