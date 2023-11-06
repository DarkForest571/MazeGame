using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Player : Creature
    {
        private char _attackImage;

        public Player(char playerImage,
                      char attackImage,
                       Vector2 position = default,
                      int health = 100,
                      float moveCoefficient = 1.0f) : base(playerImage,
                                                           position,
                                                           health,
                                                           moveCoefficient)
        {
            _attackImage = attackImage;
        }

        public override Player Clone() => new Player(Image, _attackImage, Position, Health, _moveCoefficient);

        public override void MoveTo(Direction direction, int moveCost)
        {
            base.MoveTo(direction, moveCost);
        }

        public override Projectile GetAttackProjectile() =>
            new Projectile(_attackImage, Position + AttackDirection, AttackDirection);
    }
}
