namespace MazeGame.Core
{
    class World
    {
        private Vector2 _worldSize;

        private Tile[,] _map;
        private LinkedList<Entity> _entities;

        public World(Vector2 worldSize)
        {
            _worldSize = worldSize;
            _map = new Tile[worldSize.X, worldSize.Y];
            _entities = new LinkedList<Entity>();
        }

        public Vector2 Size { get => _worldSize; }

        public Tile GetTile(int x, int y) => GetTile(new Vector2(x, y));

        public Tile GetTile(Vector2 position)
        {
            if (position > Vector2.Zero && position < _worldSize)
                return _map[position.X, position.Y];
            else
                throw new ArgumentException();
        }

        public void SetTile(int x, int y, Tile tile) => SetTile(new Vector2(x, y), tile);

        public void SetTile(Vector2 position, Tile tile)
        {
            if (position >= Vector2.Zero && position < _worldSize)
                _map[position.X, position.Y] = tile;
            else
                throw new ArgumentException();
        }
    }
}
