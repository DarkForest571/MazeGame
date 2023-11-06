using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    abstract class Creature : GameObject
    {
        private Vector2 _position;
        private int _moveTimer;
        protected readonly float _moveCoefficient;

        private int _health;
        private int _attackTimer;

        public Creature(char entityImage,
                       Vector2 position,
                       int health,
                       float moveCoefficient) : base(entityImage)
        {
            _position = position;
            _moveTimer = 0;
            _moveCoefficient = Math.Max(moveCoefficient, 0.1f);

            _health = Math.Max(health, 1);
            _attackTimer = 0;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        protected int MoveTimer => _moveTimer;

        public int Health => _health;

        protected int AttackTimer
        {
            get => _attackTimer;
            set => _attackTimer = Math.Max(value, 1);
        }

        public abstract override Creature Clone();

        public void MoveTo(Direction direction)
        {
            _position += Vector2.FromDirection(direction);
        }

        protected void UpdateMoveTimer()
        {
            if (_moveTimer > 0)
                _moveTimer--;
        }

        protected void SetMoveTimer(int frames)
        {
            _moveTimer = (int)(frames / _moveCoefficient);
        }

        public int DealDamage(int damage) => _health -= Math.Max(damage, 0);
    }
}