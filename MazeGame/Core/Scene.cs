using MazeGame.Graphics;

namespace MazeGame.Core
{
    sealed class Scene
    {
        public readonly int MAX_X;
        public readonly int MAX_Y;

        private Tile[,] _map;
        private LinkedList<Entity> _entities;

        private Generator _generator;

        private char[] _frameBuffer;

        public Scene(int xSize, int ySize, Generator generator)
        {
            (MAX_X, MAX_Y) = (xSize, ySize);
            _generator = generator;

            _map = new Tile[MAX_Y, MAX_X];
            _entities = new LinkedList<Entity>();
            _frameBuffer = new char[(MAX_X + 1) * MAX_Y];

            for (int y = 0; y < MAX_Y; y++)
                FrameBufferAt(new Vector2(MAX_X, y)) = '\n';
        }

        public Tile? TileAt(Vector2 position)
        {
            if (position.X >= 0 && position.X < MAX_X &&
                position.Y >= 0 && position.Y < MAX_Y)
            {
                return _map[position.Y, position.X];
            }
            else
                return null;
        }

        public ref char FrameBufferAt(Vector2 position)
        {
            return ref _frameBuffer[(position.Y * (MAX_X + 1)) + position.X];
        }

        public void CreateMap()
        {
            _map = _generator.Generate(MAX_X, MAX_Y);
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

        public void Render()
        {
            Console.CursorVisible = false;


            for (int yCoord = 0; yCoord < MAX_Y; ++yCoord)
                for (int xCoord = 0; xCoord < MAX_X; ++xCoord)
                    FrameBufferAt(new Vector2(xCoord, yCoord)) = _map[yCoord, xCoord].Image.Data;

            foreach (Entity entity in _entities)
            {
                FrameBufferAt(entity.Position) = entity.Image.Data;
            }

            string output = new string(_frameBuffer);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(output);
        }
    }
}
