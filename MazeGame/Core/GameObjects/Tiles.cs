namespace MazeGame.Core.GameObjects
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
        protected ImpassableTile(char image) : base(image) { }
    }
}
