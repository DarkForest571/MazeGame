using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    class Projectile : GameObject
    {
        private Vector2 _position;
        private Direction _direction;
        private readonly float _moveSpeed;
        private int _moveTimer;

        private int _lifeTime;

        private readonly int _damage;

        public Projectile(char projectileImage,
                          int distance = 1,
                          float moveSpeed = 1.0f,
                          int damage = 0,
                          Vector2 position = default,
                          Direction direction = default) : base(projectileImage)
        {
            _position = position;
            _direction = direction;
            _lifeTime = distance;
            _moveSpeed = moveSpeed;
            _damage = damage;
            _moveTimer = 0;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        public bool IsDead => _lifeTime == 0;

        public override Projectile Clone() => new Projectile(Image, _lifeTime, _moveSpeed, _damage, _position, _direction);

        public void Init(Direction direction, int framesPerSeconds)
        {
            _direction = direction;
            _moveTimer = (int)(framesPerSeconds / _moveSpeed);
        }

        public void Update(int framesPerSecond)
        {
            _moveTimer--;
            if (_moveTimer == 0)
            {
                _moveTimer = (int)(framesPerSecond / _moveSpeed);
                _position += _direction;
                _lifeTime--;
            }
        }

        public void Destroy()
        {
            _lifeTime = 0;
        }
    }
}
