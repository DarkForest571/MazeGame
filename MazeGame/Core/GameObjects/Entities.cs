using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    abstract class Entity : GameObject
    {
        private Vector2 _position;
        private int _health;
        private readonly float _moveCoefficient;

        public Entity(char entityImage,
                       Vector2 position,
                       int health,
                       float moveCoefficient) : base(entityImage)
        {
            _position = position;
            _health = health;
            _moveCoefficient = moveCoefficient;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        public float MoveCoefficient { get => _moveCoefficient; }

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
    }
}
