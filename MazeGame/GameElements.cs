using MazeGame.Core;
using MazeGame.Graphics;

namespace MazeGame
{
    class Wall : ImpassableTile
    {
        public Wall(char tileChar, int durability = -1) : base(tileChar, durability) { }
    }

    class Fence : ImpassableTile
    {
        public Fence(char tileChar, int durability = 50) : base(tileChar, durability) { }
    }

    class Space : PassableTile
    {
        public Space(char tileChar, int moveCost = 25) : base(tileChar, moveCost) { }
    }

    class Swamp : PassableTile
    {
        public Swamp(char tileChar, int moveCost = 35) : base(tileChar, moveCost) { }
    }

    class Ruins : PassableTile
    {
        public Ruins(char tileChar, int moveCost = 50) : base(tileChar, moveCost) { }
    }
}
