using MazeGame.Core.GameObjects;
using MazeGame.Graphics;

namespace MazeGame
{
    class Wall : ImpassableTile
    {
        public Wall(char tileChar, int durability = -1) : base(tileChar, durability) { }
    }

    class FinalHatch : PassableTile
    {
        public FinalHatch(char tileChar, int durability = 50) : base(tileChar, durability) { }
    }

    class Space : PassableTile
    {
        public Space(char tileChar, int moveCost = 25) : base(tileChar, moveCost) { }
    }

    class Grave : PassableTile
    {
        public Grave(char tileChar, int moveCost = 35) : base(tileChar, moveCost) { }
    }

    class Ruins : PassableTile
    {
        public Ruins(char tileChar, int moveCost = 50) : base(tileChar, moveCost) { }
    }
}
