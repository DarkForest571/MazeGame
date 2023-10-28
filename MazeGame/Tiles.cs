namespace MazeGame
{
    abstract class Tile
    {
        private Image m_tileImage;

        protected Tile(Image tileImage)
        {
            m_tileImage = tileImage;
        }

        public ref readonly Image GetImage() => ref m_tileImage;

        public abstract bool IsPassable();
    }

    abstract class PassableTile : Tile
    {
        private readonly int m_moveCost;

        protected PassableTile(Image tileImage, int moveCost) : base(tileImage)
        {
            m_moveCost = moveCost;
        }

        public override bool IsPassable() => true;

        public int GetMoveCost() => m_moveCost;
    }

    abstract class ImpassableTile : Tile
    {
        private int m_durability;

        protected ImpassableTile(Image tileImage, int durability) : base(tileImage)
        {
            m_durability = durability;
        }

        public override bool IsPassable() => false;

        public int GetDurability() => m_durability;

        public int HitTile(int damage)
        {
            if (m_durability == -1) return 1;

            return m_durability -= damage;
        }
    }

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
