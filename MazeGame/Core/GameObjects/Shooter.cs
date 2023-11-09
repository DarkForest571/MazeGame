using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Shooter : Entity/*, IAIControlable*/
    {
        Projectile _horizontalAttackProjectile;
        Projectile _verticalAttackProjectile;
        private Direction _attackDirection;

        public Shooter(char image,
                      Projectile horizontalAttackProjectile,
                      Projectile verticalAttackProjectile,
                      Vector2 position = default,
                      int health = 75,
                      float moveSpeed = 1.25f) : base(image,
                                                      position,
                                                      health,
                                                      moveSpeed)
        {
            _horizontalAttackProjectile = horizontalAttackProjectile;
            _verticalAttackProjectile = verticalAttackProjectile;
            _attackDirection = Direction.Right;

            //_idleSecondsFrom = 1;
            //_idleSecondsTo = 3;

            //_AIstate = AIState1.Idle;

            //_playerPosition = position;
        }

        public override Shooter Clone() => new Shooter(Image,
                                                       _horizontalAttackProjectile,
                                                       _verticalAttackProjectile,
                                                       Position,
                                                       Health,
                                                       MoveSpeed);


        public override Projectile? GetAttack()
        {
            if (_attackDirection == Direction.Right || _attackDirection == Direction.Left)
            {
                _horizontalAttackProjectile.Position = Position + _attackDirection;
                return _horizontalAttackProjectile.Clone();
            }
            else
            {
                _verticalAttackProjectile.Position = Position + _attackDirection;
                return _verticalAttackProjectile.Clone();
            }
        }
    }
}
