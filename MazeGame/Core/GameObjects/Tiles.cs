using static System.Net.Mime.MediaTypeNames;

namespace MazeGame.Core.GameObjects
{
    abstract class Tile : GameObject
    {
        private readonly int _moveCost;

        protected Tile(char image, int moveCost = 1) : base(image)
        {
            _moveCost = Math.Max(moveCost, 1);
        }

        public int MoveCost => _moveCost;

        public abstract bool IsPassable();
    }

    class Wall : Tile
    {
        public Wall(char tileChar) : base(tileChar) { }

        public override Wall Clone() => new Wall(Image);

        public override bool IsPassable() => false;
    }

    class FinalHatch : Tile
    {
        public FinalHatch(char tileChar, int moveCost) : base(tileChar, moveCost) { }

        public override FinalHatch Clone() => new FinalHatch(Image, MoveCost);

        public override bool IsPassable() => true;
    }

    class Space : Tile
    {
        public Space(char tileChar, int moveCost) : base(tileChar, moveCost) { }

        public override Space Clone() => new Space(Image, MoveCost);

        public override bool IsPassable() => true;
    }

    class Grave : Tile
    {
        public Grave(char tileChar, int moveCost) : base(tileChar, moveCost) { }

        public override Grave Clone() => new Grave(Image, MoveCost);

        public override bool IsPassable() => true;
    }
}
