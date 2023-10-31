namespace MazeGame.Core
{
    class World
    {
        private Vector2 _worldSize;

        private Tile[,] _map;
        private List<Entity> _entities;

        public World(Vector2 worldSize)
        {
            if (worldSize > Vector2.Zero)
                _worldSize = worldSize;
            else
                throw new ArgumentException();

            _map = new Tile[worldSize.X, worldSize.Y];
            _entities = new List<Entity>();
        }

        public Vector2 Size { get => _worldSize; }

        public Tile GetTile(int x, int y) => GetTile(new Vector2(x, y));

        public Tile GetTile(Vector2 position)
        {
            if (position >= Vector2.Zero && position < _worldSize)
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

        public int EntityCount() => _entities.Count;

        public Entity GetEntity(int index)
        {
            if (index >= 0 && index < _entities.Count)
                return _entities[index];
            else
                throw new ArgumentException();
        }
    }
}
