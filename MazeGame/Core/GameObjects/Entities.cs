using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    abstract class Entity : GameObject
    {
        private Vector2 _position;
        private int _moveTimer;
        private readonly float _moveCoefficient;

        protected Entity(char entityImage,
                       Vector2 position,
                       float moveCoefficient) : base(entityImage)
        {
            _position = position;
            _moveTimer = 0;
            _moveCoefficient = Math.Max(moveCoefficient, 0.1f);
        }

        public Vector2 Position { get => _position; set => _position = value; }

        protected int MoveTimer
        {
            get => _moveTimer;
            set => _moveTimer = (int)(value / _moveCoefficient);
        }

        public float MoveCoefficient => _moveCoefficient;

        public abstract override Creature Clone();

        public void MoveTo(Direction direction)
        {
            _position += Vector2.FromDirection(direction);
        }
    }

    abstract class Creature : Entity
    {
        private int _health;
        private int _attackTimer;

        public Creature(char entityImage,
                       Vector2 position,
                       int health,
                       float moveCoefficient) : base(entityImage, position, moveCoefficient)
        {
            _health = Math.Max(health, 1);
            _attackTimer = 0;
        }

        public int Health => _health;

        protected int AttackTimer
        {
            get => _attackTimer;
            set => _attackTimer = Math.Max(value, 1);
        }

        public abstract override Creature Clone();

        public int DealDamage(int damage) => _health -= Math.Max(damage, 0);
    }

    sealed class Player : Creature
    {
        public Player(char image,
                       Vector2 position = default,
                      int health = 100,
                      float moveCoefficient = 1.0f) : base(image,
                                                           position,
                                                           health,
                                                           moveCoefficient)
        { }

        public override Player Clone() => new Player(Image, Position, Health, MoveCoefficient);
    }

    sealed class Zombie : Creature
    {
        private int _idleFrames;
        private int _idleSecondsFrom;
        private int _idleSecondsTo;

        public Zombie(char image,
                       Vector2 position = default,
                      int health = 150,
                      float moveCoefficient = 0.75f) : base(image,
                                                           position,
                                                           health,
                                                           moveCoefficient)
        {
            _idleFrames = 0;
            _idleSecondsFrom = 2;
            _idleSecondsTo = 3;
        }

        public override Zombie Clone() => new Zombie(Image, Position, Health, MoveCoefficient);

        //public override void UpdateAI(int framesPerSecond)
        //{
        //    _idleFrames--;
        //    if (_idleFrames > 0)
        //        return;

        //    _idleFrames = GetNewIdleFrames(framesPerSecond);

        //    Direction directionToMove = Random.Shared.Next(4) switch
        //    {
        //        0 => Direction.Up,
        //        1 => Direction.Right,
        //        2 => Direction.Down,
        //        3 => Direction.Left,
        //        _ => throw new NotImplementedException()
        //    };

        //    MoveTo(directionToMove);
        //}

        //private int GetNewIdleFrames(int framesPerSecond) =>
        //    Random.Shared.Next(_idleSecondsFrom * framesPerSecond, _idleSecondsTo * framesPerSecond);
    }

    sealed class Shooter : Creature
    {
        public Shooter(char image,
                       Vector2 position = default,
                      int health = 75,
                      float moveCoefficient = 1.25f) : base(image,
                                                           position,
                                                           health,
                                                           moveCoefficient)
        {

        }

        public override Shooter Clone() => new Shooter(Image, Position, Health, MoveCoefficient);
    }
}
