using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core
{
    interface IGenerator
    {
        public void Generate();
    }

    class DefaultGenerator : IGenerator
    {
        protected World _world;

        protected readonly Tile _border;
        protected readonly Tile _filler;

        public DefaultGenerator(World world, Tile border, Tile filler)
        {
            _world = world;
            (_border, _filler) = (border, filler);
        }

        public virtual void Generate()
        {
            Box(_world, _border, Vector2.Zero, _world.Size);
            Vector2 offset = new(1, 1);
            Fill(_world, _filler, Vector2.Zero + offset, _world.Size - offset);
        }

        protected void Box(World world, Tile tile, Square squareBox)
            => Box(world, tile, squareBox.Position, squareBox.Position + squareBox.Size);

        protected void Box(World world, Tile tile, Vector2 topLeft, Vector2 bottomRight)
        {
            HorizontalLine(world, tile, topLeft, bottomRight.X - topLeft.X);
            HorizontalLine(world, tile, new(topLeft.X, bottomRight.Y - 1), bottomRight.X - topLeft.X);

            VerticalLine(world, tile, new(topLeft.X, topLeft.Y + 1), bottomRight.Y - 2 - topLeft.Y);
            VerticalLine(world, tile, new(bottomRight.X - 1, topLeft.Y + 1), bottomRight.Y - 2 - topLeft.Y);
        }

        protected void Fill(World world, Tile tile, Vector2 topLeft, Vector2 bottomRight)
        {
            int length = bottomRight.X - topLeft.X;
            for (int y = topLeft.Y, yend = bottomRight.Y; y < yend; ++y)
                HorizontalLine(world, tile, new(topLeft.X, y), length);
        }

        protected void HorizontalLine(World world, Tile tile, Vector2 position, int length)
        {
            for (int x = position.X, xend = position.X + length; x < xend; ++x)
                world[x, position.Y] = tile;
        }

        protected void VerticalLine(World world, Tile tile, Vector2 position, int length)
        {
            for (int y = position.Y, yend = position.Y + length; y < yend; ++y)
                world[position.X, y] = tile;
        }
    }

    class MazeGenerator : DefaultGenerator
    {
        public MazeGenerator(World world, Tile wall, Tile filler) : base(world, wall, filler) { }

        public override void Generate()
        {
            base.Generate();

            //if (world.Size.X % 2 == 0)
            //    VerticalLine(world, _border, new(world.Size.X - 2, 1), world.Size.Y - 2);
            //if (world.Size.Y % 2 == 0)
            //    HorizontalLine(world, _border, new(1, world.Size.Y - 2), world.Size.X - 2);

            int maxX = (_world.Size.X - 1) / 2;
            int maxY = (_world.Size.Y - 1) / 2;
            bool[,] mask = new bool[maxX, maxY];

            for (int y = 2; y < _world.Size.Y; y += 2)
                for (int x = 2; x < _world.Size.X; x += 2)
                    _world[x, y] = _border;

            bool rollBack = false;
            Vector2 position = new(1, 1);
            Stack<Direction> path = new Stack<Direction>();
            List<Direction> availableDirections = new List<Direction>(4);

            do // while (path.Count != 0)
            {
                // Check free directions
                availableDirections.Clear();

                // TODO flush this and create function for this
                if (position.Y / 2 - 1 >= 0
                    && !mask[position.X / 2, position.Y / 2 - 1]
                    && (_world[position.X, position.Y - 1] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Up);

                if (position.X / 2 + 1 < maxX
                    && !mask[position.X / 2 + 1, position.Y / 2]
                    && (_world[position.X + 1, position.Y] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Right);

                if (position.Y / 2 + 1 < maxY
                    && !mask[position.X / 2, position.Y / 2 + 1]
                    && (_world[position.X, position.Y + 1] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Down);

                if (position.X / 2 - 1 >= 0
                    && !mask[position.X / 2 - 1, position.Y / 2]
                    && (_world[position.X - 1, position.Y] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Left);

                mask[position.X / 2, position.Y / 2] = true;

                if (availableDirections.Count != 0)
                {
                    int choise = Random.Shared.Next(availableDirections.Count);

                    // Make walls
                    for (int i = 0; i < availableDirections.Count; ++i)
                    {
                        if (i != choise)
                        {
                            Vector2 newPosition = position + Vector2.FromDirection(availableDirections[i]);
                            _world[newPosition] = _border;
                        }
                    }

                    // Move position
                    Vector2 directionVector = Vector2.FromDirection(availableDirections[choise]);
                    _world[position + directionVector] = _filler;
                    position += directionVector * 2;

                    path.Push(availableDirections[choise]);
                    rollBack = false;
                }
                else
                {
                    position -= Vector2.FromDirection(path.Pop()) * 2;

                    rollBack = true;
                }
            } while (path.Count != 0);
        }
    }
}
