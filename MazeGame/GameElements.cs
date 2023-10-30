using MazeGame.Core;
using MazeGame.Graphics;

namespace MazeGame
{
    class Wall : ImpassableTile
    {
        public Wall(IImage tileImage, int durability = -1) : base(tileImage, durability) { }
    }

    class Fence : ImpassableTile
    {
        public Fence(IImage tileImage, int durability = 50) : base(tileImage, durability) { }
    }

    class Space : PassableTile
    {
        public Space(IImage tileImage, int moveCost = 25) : base(tileImage, moveCost) { }
    }

    class Swamp : PassableTile
    {
        public Swamp(IImage tileImage, int moveCost = 35) : base(tileImage, moveCost) { }
    }

    class Ruins : PassableTile
    {
        public Ruins(IImage tileImage, int moveCost = 50) : base(tileImage, moveCost) { }
    }
}
