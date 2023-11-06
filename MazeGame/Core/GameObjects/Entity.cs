using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    abstract class Entity : GameObject
    {
        private Vector2 _position;
        private int _moveTimer;
        protected readonly float _moveCoefficient;

        private Direction _attackDirection;
        private int _health;
        private int _attackTimer;

        public Entity(char entityImage,
                       Vector2 position,
                       int health,
                       float moveSpeed) : base(entityImage)
        {
            _position = position;
            _moveTimer = 0;
            _moveCoefficient = Math.Max(moveSpeed, 0.1f);

            _attackDirection = Direction.None;
            _health = Math.Max(health, 1);
            _attackTimer = 0;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        protected int MoveTimer => _moveTimer;

        protected Direction AttackDirection => _attackDirection;

        public int Health => _health;

        protected int AttackTimer
        {
            get => _attackTimer;
            set => _attackTimer = Math.Max(value, 1);
        }

        public abstract override Entity Clone();

        public virtual void MoveTo(Direction direction, int moveCost)
        {
            if (_moveTimer == 0)
            {
                _moveTimer = (int)(Math.Max(moveCost, 1) / _moveCoefficient);
                _position += direction;
            }
        }

        public void UpdateMoveTimer()
        {
            if (_moveTimer > 0)
                _moveTimer--;
        }

        public int TakeDamage(int damage) => _health -= Math.Max(damage, 0);

        public abstract Projectile GetAttackProjectile();
    }
}