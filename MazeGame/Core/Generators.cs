﻿namespace MazeGame.Core
{
    interface Generator
    {
        public void Generate(World world);
    }

    class DefaultGenerator : Generator
    {
        protected readonly Tile _border;
        protected readonly Tile _filler;

        public DefaultGenerator(Tile border, Tile filler)
        {
            (_border, _filler) = (border, filler);
        }

        public virtual void Generate(World world)
        {
            Box(world, _border, Vector2.Zero, world.Size);
            Vector2 offset = new(1, 1);
            Fill(world, _filler, Vector2.Zero + offset, world.Size - offset);
        }

        protected void Box(World world, Tile tile, Square squareBox)
            => Box(world, tile, squareBox.Position, squareBox.Position + squareBox.Size);

        protected void Box(World world, Tile tile, Vector2 upperLeft, Vector2 bottomRight)
        {
            HorizontalLine(world, tile, upperLeft, bottomRight.X - upperLeft.X);
            HorizontalLine(world, tile, new(upperLeft.X, bottomRight.Y - 1), bottomRight.X - upperLeft.X);

            VerticalLine(world, tile, new(upperLeft.X, upperLeft.Y + 1), bottomRight.Y - 2 - upperLeft.Y);
            VerticalLine(world, tile, new(bottomRight.X - 1, upperLeft.Y + 1), bottomRight.Y - 2 - upperLeft.Y);
        }

        protected void Fill(World world, Tile tile, Vector2 upperLeft, Vector2 bottomRight)
        {
            int length = bottomRight.X - upperLeft.X;
            for (int y = upperLeft.Y, yend = bottomRight.Y; y < yend; ++y)
                HorizontalLine(world, tile, new(upperLeft.X, y), length);
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
        public MazeGenerator(Tile wall, Tile filler) : base(wall, filler) { }

        public override void Generate(World world)
        {
            base.Generate(world);

            //if (world.Size.X % 2 == 0)
            //    VerticalLine(world, _border, new(world.Size.X - 2, 1), world.Size.Y - 2);
            //if (world.Size.Y % 2 == 0)
            //    HorizontalLine(world, _border, new(1, world.Size.Y - 2), world.Size.X - 2);

            //Box(world, _border, world.Size * 0.25f, world.Size * 0.75f);

            int maxX = (world.Size.X - 1) / 2;
            int maxY = (world.Size.Y - 1) / 2;
            bool[,] mask = new bool[maxX, maxY];

            for (int y = 2; y < world.Size.Y; y += 2)
                for (int x = 2; x < world.Size.X; x += 2)
                    world[x, y] = _border;

            bool rollBack = false;
            Vector2 position = new(1, 1);
            Stack<Direction> path = new Stack<Direction>();
            List<Direction> availableDirections = new List<Direction>(4);

            do // while (path.Count != 0)
            {
                // Check frees
                availableDirections.Clear();

                if (position.Y / 2 - 1 >= 0
                    && !mask[position.X / 2, position.Y / 2 - 1]
                    && (world[position.X, position.Y - 1] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Up);

                if (position.X / 2 + 1 < maxX
                    && !mask[position.X / 2 + 1, position.Y / 2]
                    && (world[position.X + 1, position.Y] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Right);

                if (position.Y / 2 + 1 < maxY
                    && !mask[position.X / 2, position.Y / 2 + 1]
                    && (world[position.X, position.Y + 1] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Bottom);

                if (position.X / 2 - 1 >= 0
                    && !mask[position.X / 2 - 1, position.Y / 2]
                    && (world[position.X - 1, position.Y] is PassableTile || rollBack))
                    availableDirections.Add(Direction.Left);

                mask[position.X / 2, position.Y / 2] = true;

                if (availableDirections.Count != 0)
                {
                    // Make walls

                    int choise = Random.Shared.Next(availableDirections.Count);

                    for (int i = 0; i < availableDirections.Count; ++i)
                    {
                        if (i != choise)
                        {
                            switch (availableDirections[i])
                            {
                                case Direction.Up:
                                    world[position.X, position.Y - 1] = _border;
                                    break;
                                case Direction.Right:
                                    world[position.X + 1, position.Y] = _border;
                                    break;
                                case Direction.Bottom:
                                    world[position.X, position.Y + 1] = _border;
                                    break;
                                case Direction.Left:
                                    world[position.X - 1, position.Y] = _border;
                                    break;
                            }
                        }
                    }

                    // Move position

                    switch (availableDirections[choise])
                    {
                        case Direction.Up:
                            if (rollBack)
                                world[position.X, position.Y - 1] = _filler;
                            position.Y -= 2;
                            break;
                        case Direction.Right:
                            if (rollBack)
                                world[position.X + 1, position.Y] = _filler;
                            position.X += 2;
                            break;
                        case Direction.Bottom:
                            if (rollBack)
                                world[position.X, position.Y + 1] = _filler;
                            position.Y += 2;
                            break;
                        case Direction.Left:
                            if (rollBack)
                                world[position.X - 1, position.Y] = _filler;
                            position.X -= 2;
                            break;
                    }

                    path.Push(availableDirections[choise]);

                    rollBack = false;
                }
                else
                {
                    switch (path.Pop())
                    {
                        case Direction.Up:
                            position.Y += 2;
                            break;
                        case Direction.Right:
                            position.X -= 2;
                            break;
                        case Direction.Bottom:
                            position.Y -= 2;
                            break;
                        case Direction.Left:
                            position.X += 2;
                            break;
                    }

                    rollBack = true;
                }
            } while (path.Count != 0);
        }
        enum Direction
        {
            Up,
            Right,
            Bottom,
            Left,
            None
        }
    }
}
