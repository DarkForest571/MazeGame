using System.Collections.ObjectModel;

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

        public Tile this[int x, int y]
        {
            get => _map[x, y];
            set => _map[x, y] = value;
        }

        public Tile this[Vector2 position]
        {
            get => _map[position.X, position.Y];
            set => _map[position.X, position.Y] = value;
        }

        public int EntityCount() => _entities.Count;

        public ReadOnlyCollection<Entity> Entities() => _entities.AsReadOnly();
    }
}
