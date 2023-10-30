namespace MazeGame.Core
{
    interface Generator
    {
        public void Generate(World world);
    }

    class DefaultGenerator : Generator
    {
        protected Tile _border;
        protected Tile _filler;

        public DefaultGenerator(Tile border, Tile filler)
        {
            _border = border;
            _filler = filler;
        }

        public virtual void Generate(World world)
        {
            int maxX = world.Size.X;
            int maxY = world.Size.Y;

            for (int x = 0; x < maxX; ++x)
                world.Map[x, 0] = world.Map[x, maxY - 1] = _border;
            for (int y = 1, end = maxY - 1; y < end; ++y)
                world.Map[0, y] = world.Map[maxX - 1, y] = _border;

            for (int y = 1, yend = maxY - 1; y < yend; ++y)
                for (int x = 1, xend = maxX - 1; x < xend; ++x)
                    world.Map[x, y] = _filler;
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
