using static System.Net.Mime.MediaTypeNames;

namespace MazeGame.Core.GameObjects
{
    class Tile : GameObject
    {
        private readonly bool _isPassable;
        private readonly float _moveCost;

        public Tile(char image, bool isPassable, float moveCost = 1f) : base(image)
        {
            _isPassable = isPassable;
            _moveCost = Math.Max(moveCost, 0.001f);
        }

        public float MoveCost => _moveCost;

        public bool IsPassable => _isPassable;

        public override Tile Clone() => new Tile(Image, IsPassable, MoveCost);
    }
}
