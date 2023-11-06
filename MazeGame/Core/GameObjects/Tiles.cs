using static System.Net.Mime.MediaTypeNames;

namespace MazeGame.Core.GameObjects
{
    abstract class Tile : GameObject
    {
        private readonly int _moveCost;

        protected Tile(char image, int moveCost = 100) : base(image)
        {
            _moveCost = moveCost;
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
        public FinalHatch(char tileChar, int moveCost = 50) : base(tileChar, moveCost) { }

        public override FinalHatch Clone() => new FinalHatch(Image);

        public override bool IsPassable() => true;
    }

    class Space : Tile
    {
        public Space(char tileChar, int moveCost = 25) : base(tileChar, moveCost) { }

        public override Space Clone() => new Space(Image);

        public override bool IsPassable() => true;
    }

    class Grave : Tile
    {
        public Grave(char tileChar, int moveCost = 35) : base(tileChar, moveCost) { }

        public override Grave Clone() => new Grave(Image);

        public override bool IsPassable() => true;
    }
}
