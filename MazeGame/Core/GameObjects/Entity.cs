using MazeGame.Utils;
using System.Runtime.InteropServices;

namespace MazeGame.Core.GameObjects
{
    abstract class Entity : GameObject
    {
        private Vector2 _position;
        private readonly float _moveSpeed;
        private int _moveTimer;

        private int _attackTimer;

        private int _health;

        public Entity(char entityImage,
                      Vector2 position,
                      int health,
                      float moveSpeed) : base(entityImage)
        {
            _position = position;
            _moveSpeed = Math.Max(moveSpeed, 0.1f);
            _moveTimer = 0;

            _attackTimer = 0;

            _health = Math.Max(health, 1);
        }

        public Vector2 Position { get => _position; set => _position = value; }

        protected float MoveSpeed => _moveSpeed;

        protected int MoveTimer => _moveTimer;

        protected int AttackTimer
        {
            get => _attackTimer;
            set => _attackTimer = Math.Max(value, 1);
        }

        public int Health => _health;

        public abstract override Entity Clone();

        public abstract Projectile? GetAttack();

        public virtual void MoveTo(Direction direction, int moveCost)
        {
            if (_moveTimer == 0)
            {
                _moveTimer = (int)(Math.Max(moveCost, 1) / _moveSpeed);
                _position += direction;
            }
        }

        public void UpdateMoveTimer()
        {
            if (_moveTimer > 0)
                _moveTimer--;
        }

        public int TakeDamage(int damage) => _health -= Math.Max(damage, 0);
    }
}