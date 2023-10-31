namespace MazeGame.Core
{
    interface Generator
    {
        public void Generate(World world);
    }

    class DefaultGenerator : Generator
    {
        protected readonly Tile _border;
        protected readonly Tile _filler;

        public DefaultGenerator(Tile border, Tile filler)
        {
            (_border, _filler) = (border, filler);
        }

        public virtual void Generate(World world)
        {
            HorizontalLine(world, _border, new(0, 0), world.Size.X);
            HorizontalLine(world, _border, new(0, world.Size.Y - 1), world.Size.X);

            VerticalLine(world, _border, new(0, 1), world.Size.Y - 2);
            VerticalLine(world, _border, new(world.Size.X - 1, 1), world.Size.Y - 2);

            Fill(world, _filler, new(1, 1), world.Size - new Vector2(1,1));
        }

        protected void Fill(World world, Tile tile, Vector2 upperLeft, Vector2 bottomRight)
        {
            int length = bottomRight.X - upperLeft.X;
            for (int y = upperLeft.Y, yend = bottomRight.Y; y < yend; ++y)
                HorizontalLine(world, tile, new(upperLeft.X, y), length);
        }

        protected void HorizontalLine(World world, Tile tile, Vector2 position, int length)
        {
            for (int x = position.X, xend = position.X + length; x < xend; ++x)
                world[x, position.Y] = tile;
        }

        protected void VerticalLine(World world, Tile tile, Vector2 position, int length)
        {
            for (int y = position.Y, yend = position.Y + length; y < yend; ++y)
                world[position.X, y] = tile;
        }
    }

    class MazeGenerator : DefaultGenerator
    {
        public MazeGenerator(Tile wall, Tile filler) : base(wall, filler) { }

        public override void Generate(World world)
        {
            base.Generate(world);

            // TODO Make maze here
        }
    }
}
