using MazeGame.Graphics;

namespace MazeGame.Core
{
    abstract class Tile : GameObject
    {
        protected Tile(char image) : base(image) { }
    }

    abstract class PassableTile : Tile
    {
        private readonly int _moveCost;

        protected PassableTile(char image, int moveCost) : base(image)
        {
            _moveCost = moveCost;
        }

        public int MoveCost { get => _moveCost; }
    }

    abstract class ImpassableTile : Tile
    {
        private int _durability;

        protected ImpassableTile(char image, int durability) : base(image)
        {
            _durability = durability;
        }

        public int Durability { get => _durability; private set => _durability = value; }

        public int HitTile(int damage) => _durability == -1 ? 1 : _durability -= damage;
    }
}
