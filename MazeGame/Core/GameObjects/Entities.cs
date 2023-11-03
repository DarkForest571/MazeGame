using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    abstract class Entity : GameObject
    {
        private Vector2 _position;
        private int _health;
        private readonly float _moveCoefficient;
        private int _moveTimer;
        private int _attackTimer;

        public Entity(char entityImage,
                       Vector2 position,
                       int health,
                       float moveCoefficient) : base(entityImage)
        {
            _position = position;
            _health = health;
            _moveCoefficient = moveCoefficient;
            _moveTimer = 0;
            _attackTimer = 0;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        public int Health => _health;

        public float MoveCoefficient => _moveCoefficient;

        public abstract override Entity Clone();

        public int HitEntity(int damage) => _health -= damage;
    }

    sealed class Player : Entity
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

    sealed class Zombie : Entity
    {
        public Zombie(char image,
                       Vector2 position = default,
                      int health = 150,
                      float moveCoefficient = 0.75f) : base(image,
                                                           position,
                                                           health,
                                                           moveCoefficient)
        { }

        public override Zombie Clone() => new Zombie(Image, Position, Health, MoveCoefficient);
    }

    sealed class Shooter : Entity
    {
        public Shooter(char image,
                       Vector2 position = default,
                      int health = 75,
                      float moveCoefficient = 1.25f) : base(image,
                                                           position,
                                                           health,
                                                           moveCoefficient)
        { }

        public override Shooter Clone() => new Shooter(Image, Position, Health, MoveCoefficient);
    }
}
