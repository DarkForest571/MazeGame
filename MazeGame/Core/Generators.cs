namespace MazeGame.Core
{
    interface Generator
    {
        public Tile[,] Generate(int width, int height);
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

        public virtual Tile[,] Generate(int width, int height)
        {
            Tile[,] map = new Tile[height, width];

            for (int y = 0; y < height; ++y)
                map[y, 0] = map[y, width - 1] = _border;
            for (int x = 1, end = width - 1; x < end; ++x)
                map[0, x] = map[height - 1, x] = _border;

            for (int y = 1, yend = height - 1; y < yend; ++y)
                for (int x = 1, xend = width - 1; x < xend; ++x)
                    map[y, x] = _filler;

            return map;
        }
    }

    class MazeGenerator : DefaultGenerator
    {
        public MazeGenerator(Tile wall, Tile filler) : base(wall, filler) { }

        public override Tile[,] Generate(int width, int height)
        {
            Tile[,] map = base.Generate(width, height);

            // TODO Make maze here

            return map;
        }
    }
}
