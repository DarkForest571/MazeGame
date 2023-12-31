﻿namespace MazeGame.Utils
{
    struct Vector2
    {
        public int X, Y;

        public Vector2(int x = 0, int y = 0)
        {
            (X, Y) = (x, y);
        }

        private static Vector2 _zero = new Vector2(0, 0);

        private static Vector2 _up = new Vector2(0, -1);
        private static Vector2 _right = new Vector2(1, 0);
        private static Vector2 _down = new Vector2(0, 1);
        private static Vector2 _left = new Vector2(-1, 0);

        public static Vector2 Zero => _zero;

        public static Vector2 Up => _up;
        public static Vector2 Right => _right;
        public static Vector2 Left => _left;
        public static Vector2 Down => _down;

        public static bool operator <(Vector2 left, Vector2 right) => left.X < right.X && left.Y < right.Y;

        public static bool operator >(Vector2 left, Vector2 right) => left.X > right.X && left.Y > right.Y;

        public static bool operator <=(Vector2 left, Vector2 right) => left.X <= right.X && left.Y <= right.Y;

        public static bool operator >=(Vector2 left, Vector2 right) => left.X >= right.X && left.Y >= right.Y;

        public static bool operator ==(Vector2 left, Vector2 right) => left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);


        public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

        public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

        public static Vector2 operator +(Vector2 left, Direction right) =>
            new(left.X + FromDirection(right).X, left.Y + FromDirection(right).Y);


        public static Vector2 operator *(Vector2 left, int right) => new(left.X * right, left.Y * right);

        public static Vector2 operator *(int left, Vector2 right) => right * left;

        public static Vector2 operator /(Vector2 left, int right) => new(left.X / right, left.Y / right);


        public static Vector2 operator *(Vector2 left, float right) => new((int)(left.X * right), (int)(left.Y * right));

        public static Vector2 operator *(float left, Vector2 right) => right * left;

        public static Vector2 operator /(Vector2 left, float right) => new((int)(left.X / right), (int)(left.Y / right));


        public static int SqDistance(Vector2 left, Vector2 right)
        {
            int x = left.X - right.X;
            int y = left.Y - right.Y;
            return x * x + y * y;
        }


        public static Direction GetDirection(Vector2 position, Vector2 target)
        {
            if (position == target)
                return Direction.None;

            Vector2 delta = target - position;

            if (delta.Y == 0 || Math.Abs(delta.X) > Math.Abs(delta.Y))
            {
                if (delta.X > 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }

            if (delta.X == 0 || Math.Abs(delta.X) <= Math.Abs(delta.Y))
            {
                if (delta.Y > 0)
                    return Direction.Down;
                else
                    return Direction.Up;
            }
            throw new NotImplementedException();
        }

        public static Vector2 FromDirection(Direction direction) =>
            direction switch
            {
                Direction.Up => Up,
                Direction.Right => Right,
                Direction.Down => Down,
                Direction.Left => Left,
                Direction.None => Zero,
                _ => throw new ArgumentException()
            };

        public void Deconstruct(out int X, out int Y)
        {
            throw new NotImplementedException();
        }
    }
}
