using MazeGame.Graphics;

namespace MazeGame.Core
{
    sealed class Scene
    {
        public readonly int WIDTH;
        public readonly int HEIGHT;

        private Tile[,] _map;
        private LinkedList<Entity> _entities;

        private Generator _generator;

        public Scene(int width, int height, Generator generator)
        {
            (WIDTH, HEIGHT) = (width, height);
            _generator = generator;

            _map = new Tile[WIDTH, HEIGHT];
            _entities = new LinkedList<Entity>();
        }

        public Tile? TileAt(Vector2 position)
        {
            if (position.X >= 0 && position.X < WIDTH &&
                position.Y >= 0 && position.Y < HEIGHT)
            {
                return _map[position.X, position.Y];
            }
            else
                return null;
        }

        public void CreateMap()
        {
            _map = _generator.Generate(WIDTH, HEIGHT);
        }

        // Entitiy manager

        public bool PlaceEntity(Entity entity)
        {
            if (TileAt(entity.Position) is PassableTile)
            {
                _entities.AddLast(entity);
                return true;
            }
            return false;
        }

        // Renderer


    }
}
