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
            Box(world, _border, Vector2.Zero, world.Size);
            Vector2 offset = new(1, 1);
            Fill(world, _filler, Vector2.Zero + offset, world.Size - offset);
        }

        protected void Box(World world, Tile tile, Square squareBox)
            => Box(world, tile, squareBox.Position, squareBox.Position + squareBox.Size);

        protected void Box(World world, Tile tile, Vector2 upperLeft, Vector2 bottomRight)
        {
            HorizontalLine(world, tile, upperLeft, bottomRight.X - upperLeft.X);
            HorizontalLine(world, tile, new(upperLeft.X, bottomRight.Y - 1), bottomRight.X - upperLeft.X);

            VerticalLine(world, tile, new(upperLeft.X, upperLeft.Y + 1), bottomRight.Y - 2 - upperLeft.Y);
            VerticalLine(world, tile, new(bottomRight.X - 1, upperLeft.Y + 1), bottomRight.Y - 2 - upperLeft.Y);
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

            Box(world, _border, world.Size * 0.25f, world.Size * 0.75f);

            // TODO Make maze here
        }
    }
}
