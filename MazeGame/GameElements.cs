using MazeGame.Core;
using MazeGame.Graphics;

namespace MazeGame
{
    class Wall : ImpassableTile
    {
        public Wall(char tileImage, int durability = -1) : base(tileImage, durability) { }
    }

    class Fence : ImpassableTile
    {
        public Fence(char tileImage, int durability = 50) : base(tileImage, durability) { }
    }

    class Space : PassableTile
    {
        public Space(char tileImage, int moveCost = 25) : base(tileImage, moveCost) { }
    }

    class Swamp : PassableTile
    {
        public Swamp(char tileImage, int moveCost = 35) : base(tileImage, moveCost) { }
    }

    class Ruins : PassableTile
    {
        public Ruins(char tileImage, int moveCost = 50) : base(tileImage, moveCost) { }
    }
}
