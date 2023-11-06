using static System.Net.Mime.MediaTypeNames;

namespace MazeGame.Core.GameObjects
{
    class Tile : GameObject
    {
        private readonly bool _isPassable;
        private readonly int _moveCost;

        public Tile(char image, bool isPassable, int moveCost = 1) : base(image)
        {
            _isPassable = isPassable;
            _moveCost = Math.Max(moveCost, 1);
        }

        public int MoveCost => _moveCost;

        public bool IsPassable => _isPassable;

        public override Tile Clone() => new Tile(Image, IsPassable, MoveCost);
    }
}
