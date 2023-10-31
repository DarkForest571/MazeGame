﻿namespace MazeGame.Core
{
    struct Vector2
    {
        public int X, Y;

        public Vector2(int x = 0, int y = 0)
        {
            (X, Y) = (x, y);
        }

        private static Vector2 _zero = new Vector2(0, 0);

        public static Vector2 Zero => _zero;

        public static bool operator <(Vector2 left, Vector2 right) => left.X < right.X && left.Y < right.Y;

        public static bool operator >(Vector2 left, Vector2 right) => left.X > right.X && left.Y > right.Y;

        public static bool operator <=(Vector2 left, Vector2 right) => left.X <= right.X && left.Y <= right.Y;

        public static bool operator >=(Vector2 left, Vector2 right) => left.X >= right.X && left.Y >= right.Y;

        public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

        public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

        public void Deconstruct(out int X, out int Y)
        {
            throw new NotImplementedException();
        }
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
