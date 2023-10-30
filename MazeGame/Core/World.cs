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

        public Tile? TileAt(Vector2 position)
        {
            if (position.X >= 0 && position.X < _worldSize.X &&
                position.Y >= 0 && position.Y < _worldSize.Y)
            {
                return _map[position.X, position.Y];
            }
            else
                return null;
        }
    }
}
