using MazeGame.Graphics;

namespace MazeGame.Core
{
    abstract class Entity : GameObject
    {
        private Vector2 _position;
        private int _health;
        private readonly float _moveCoefficient;

        public Entity(IImage image,
                       Vector2 position,
                       int health,
                       float moveCoefficient) : base(image)
        {
            _position = position;
            _health = health;
            _moveCoefficient = moveCoefficient;
        }

        public Vector2 Position { get => _position; set => _position = value; }

        public float MoveCoefficient { get => _moveCoefficient; }

        public int HitEntity(int damage) => _health -= damage;
    }

    class Player : Entity
    {
        public Player(IImage image,
                       Vector2 position = new Vector2(),
                      int health = 100,
                      float moveCoefficient = 1.0f) : base(image,
                                                           position,
                                                           health,
                                                           moveCoefficient) { }
    }
}
