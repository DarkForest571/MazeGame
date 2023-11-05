using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    abstract class Entity : GameObject
    {
        private Vector2 _position;
        private int _moveTimer;
        protected readonly float _moveCoefficient;

        protected Entity(char entityImage,
                       Vector2 position,
                       float moveCoefficient) : base(entityImage)
        {
            _position = position;
            _moveTimer = 0;
            _moveCoefficient = Math.Max(moveCoefficient, 0.1f);
        }

        public Vector2 Position { get => _position; set => _position = value; }

        protected int MoveTimer => _moveTimer;

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

        public override Player Clone() => new Player(Image, Position, Health, _moveCoefficient);
    }

    sealed class Zombie : Creature, IAIControlable
    {
        private int _idleFrames;
        private int _idleSecondsFrom;
        private int _idleSecondsTo;

        private Vector2 _targetPosition;

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
            _idleSecondsTo = 5;

            _targetPosition = position;
        }

        public override Zombie Clone() => new Zombie(Image, Position, Health, _moveCoefficient);

        public void UpdateAI(World world, Player player, int framesPerSecond)
        {
            _idleFrames--;
            if (_idleFrames > 0)
                return;

            _idleFrames = GetNewIdleFrames(framesPerSecond);

            bool visible = false;
            if (player.Position.X == Position.X)
            {
                visible = true;
                int min = Math.Min(player.Position.Y, Position.Y);
                int max = Math.Max(player.Position.Y, Position.Y);
                for (; min < max; min++)
                {
                    if (world[Position.X, min] is ImpassableTile)
                    {
                        visible = false;
                        break;
                    }
                }
            }
            else if (player.Position.Y == Position.Y)
            {
                visible = true;
                int min = Math.Min(player.Position.X, Position.X);
                int max = Math.Max(player.Position.X, Position.X);
                for (; min < max; min++)
                {
                    if (world[min, Position.Y] is ImpassableTile)
                    {
                        visible = false;
                        break;
                    }
                }
            }

            if (visible)
                _targetPosition = player.Position;
        }

        public void AIAction(World world, Player player, int framesPerSecond)
        {
            UpdateMoveTimer();
            Direction moveDirection = Vector2.GetDirection(Position, _targetPosition);

            if (Position + Vector2.FromDirection(moveDirection) == _targetPosition &&
                _targetPosition == player.Position)
            {
                // Hit
            }
            else
            {
                if (MoveTimer == 0)
                {
                    MoveTo(moveDirection);
                    SetMoveTimer(((PassableTile)world[Position]).MoveCost);
                }
            }
        }

        private int GetNewIdleFrames(int framesPerSecond) =>
            Random.Shared.Next(_idleSecondsFrom * framesPerSecond, _idleSecondsTo * framesPerSecond);
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

        public override Shooter Clone() => new Shooter(Image, Position, Health, _moveCoefficient);
    }
}
