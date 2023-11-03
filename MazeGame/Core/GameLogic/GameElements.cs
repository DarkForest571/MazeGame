using MazeGame.Core.GameObjects;

namespace MazeGame.Core.GameLogic
{
    class Wall : ImpassableTile
    {
        public Wall(char tileChar) : base(tileChar) { }

        public override Wall Clone() => new Wall(Image);
    }

    class FinalHatch : PassableTile
    {
        public FinalHatch(char tileChar, int moveCost = 50) : base(tileChar, moveCost) { }

        public override FinalHatch Clone() => new FinalHatch(Image);
    }

    class Space : PassableTile
    {
        public Space(char tileChar, int moveCost = 25) : base(tileChar, moveCost) { }

        public override Space Clone() => new Space(Image);
    }

    class Grave : PassableTile
    {
        public Grave(char tileChar, int moveCost = 35) : base(tileChar, moveCost) { }

        public override Grave Clone() => new Grave(Image);
    }
}
