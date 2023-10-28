using MazeGame.Core;

namespace MazeGame
{
    class Wall : ImpassableTile
    {
        public Wall(Image tileImage, int durability = -1) : base(tileImage, durability) { }
    }

    class Fence : ImpassableTile
    {
        public Fence(Image tileImage, int durability = 50) : base(tileImage, durability) { }
    }

    class Space : PassableTile
    {
        public Space(Image tileImage, int moveCost = 25) : base(tileImage, moveCost) { }
    }

    class Swamp : PassableTile
    {
        public Swamp(Image tileImage, int moveCost = 35) : base(tileImage, moveCost) { }
    }

    class Ruins : PassableTile
    {
        public Ruins(Image tileImage, int moveCost = 50) : base(tileImage, moveCost) { }
    }
}
