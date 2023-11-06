using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
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
}
