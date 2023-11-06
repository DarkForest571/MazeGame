using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    class Projectile : GameObject
    {
        private Vector2 _position;
        private Direction _direction;
        private int _lifeTime;
        private int _damage;

        public Projectile(char projectileImage,
                          Vector2 position = default,
                          Direction direction = default,
                          int lifeTime = 1,
                          int damage = 0) : base(projectileImage)
        {
            _position = position;
            _direction = direction;
            _lifeTime = lifeTime;
            _damage = damage;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        public override Projectile Clone() => new Projectile(Image, _position, _direction, _lifeTime);
    }
}
