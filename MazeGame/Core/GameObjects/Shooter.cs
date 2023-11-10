using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Shooter : Entity/*, IAIControlable*/
    {
        Projectile _horizontalAttackProjectile;
        Projectile _verticalAttackProjectile;

        public Shooter(char image,
                      Projectile horizontalAttackProjectile,
                      Projectile verticalAttackProjectile,
                      int health = 75,
                      float moveSpeed = 1.5f,
                      Vector2 position = default) : base(image,
                                                         health,
                                                         moveSpeed,
                                                         position)
        {
            _horizontalAttackProjectile = horizontalAttackProjectile;
            _verticalAttackProjectile = verticalAttackProjectile;

            //_idleSecondsFrom = 1;
            //_idleSecondsTo = 3;

            //_AIstate = AIState1.Idle;

            //_playerPosition = position;
        }

        public override Shooter Clone() => new Shooter(Image,
                                                       _horizontalAttackProjectile,
                                                       _verticalAttackProjectile,
                                                       Health,
                                                       MoveSpeed,
                                                       Position);


        public override Projectile? GetAttack()
        {
            return null;
            //if (_attackDirection == Direction.Right || _attackDirection == Direction.Left)
            //{
            //    _horizontalAttackProjectile.Position = Position + _attackDirection;
            //    return _horizontalAttackProjectile.Clone();
            //}
            //else
            //{
            //    _verticalAttackProjectile.Position = Position + _attackDirection;
            //    return _verticalAttackProjectile.Clone();
            //}
        }
    }
}
