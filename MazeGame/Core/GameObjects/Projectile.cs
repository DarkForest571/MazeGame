using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    class Projectile : GameObject
    {
        private Vector2 _position;
        private Direction _direction;
        private readonly float _secondsPerTile;
        private int _moveTimer;

        private int _lifeTime;

        private readonly int _damage;
        private Entity _owner;

        public Projectile(char projectileImage,
                          int distance = 1,
                          float secondsPerTile = 1.0f,
                          int damage = 0,
                          Vector2 position = default,
                          Direction direction = default) : base(projectileImage)
        {
            _position = position;
            _direction = direction;
            _secondsPerTile = secondsPerTile;
            _moveTimer = 0;

            _lifeTime = distance;

            _damage = damage;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        public int Damage => _damage;

        public Entity Owner => _owner;

        public bool IsDead => _lifeTime == 0;

        public override Projectile Clone() => new Projectile(Image, _lifeTime, _secondsPerTile, _damage, _position, _direction);

        public void Init(Entity owner, Direction direction, int framesPerSeconds)
        {
            _owner = owner;
            _direction = direction;
            _moveTimer = (int)(framesPerSeconds * _secondsPerTile);
        }

        public void Update(int framesPerSecond)
        {
            _moveTimer--;
            if (_moveTimer == 0)
            {
                _moveTimer = (int)(framesPerSecond * _secondsPerTile);
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
