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

        //public Tile[,] Map { get => _map; }

        public ref Tile TileAt(Vector2 position)
        {
            if (position.IsPositive() && position.X < _worldSize.X && position.Y < _worldSize.Y)
            {
                return ref _map[position.X, position.Y];
            }
            else
                throw new ArgumentException();
        }

        public ref Tile TileAt(int x, int y) => ref TileAt(new Vector2(x, y));
    }
}
