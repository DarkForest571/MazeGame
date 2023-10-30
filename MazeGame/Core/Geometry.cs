namespace MazeGame.Core
{
    struct Vector2
    {
        public int X, Y;

        public Vector2(int x = 0, int y = 0)
        {
            (X, Y) = (x, y);
        }

        public bool IsPositive() => X >= 0 && Y >= 0;
    }

    struct Square
    {
        public Vector2 Position;
        public Vector2 Size;

        public Square(Vector2 size, Vector2 position = new Vector2())
        {
            Position = position;
            Size = size;
        }
    }
}
