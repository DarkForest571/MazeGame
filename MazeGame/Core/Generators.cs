namespace MazeGame.Core
{
    interface Generator
    {
        public Tile[,] Generate(int xSize, int ySize);
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

        public virtual Tile[,] Generate(int xSize, int ySize)
        {
            Tile[,] map = new Tile[ySize, xSize];

            for (int y = 0; y < ySize; ++y)
                map[y, 0] = map[y, xSize - 1] = _border;
            for (int x = 1, end = xSize - 1; x < end; ++x)
                map[0, x] = map[ySize - 1, x] = _border;

            for (int y = 1, yend = ySize - 1; y < yend; ++y)
                for (int x = 1, xend = xSize - 1; x < xend; ++x)
                    map[y, x] = _filler;

            return map;
        }
    }

    class MazeGenerator : DefaultGenerator
    {
        public MazeGenerator(Tile wall, Tile filler) : base(wall, filler) { }

        public override Tile[,] Generate(int xSize, int ySize)
        {
            Tile[,] map = base.Generate(xSize, ySize);

            // TODO Make maze here

            return map;
        }
    }
}
