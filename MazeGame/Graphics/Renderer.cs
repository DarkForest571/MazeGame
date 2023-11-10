using MazeGame.Core;
using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Graphics
{
    abstract class BasicRenderer
    {
        protected Square _frame;

        protected char _clearChar;
        protected char[] _frameBuffer;

        protected BasicRenderer(Square frame, char clearChar = '?')
        {
            _frame = frame;
            _clearChar = clearChar;
            _frameBuffer = new char[frame.Size.X * frame.Size.Y];
        }

        public Square Frame { get => _frame; set => _frame = value; }

        public Vector2 Size => _frame.Size;

        public Vector2 Position { get => _frame.Position; set => _frame.Position = value; }

        public void Render()
        {
            for (int y = 0; y < Size.Y; ++y)
            {
                Console.SetCursorPosition(Position.X, Position.Y + y);
                Console.Write(_frameBuffer, y * Size.X, Size.X);
            }
        }

        public void ClearBuffer()
        {
            for (int y = 0; y < Size.Y; ++y)
                for (int x = 0; x < Size.X; ++x)
                    _frameBuffer[y * Size.X + x] = _clearChar;
        }
    }

    sealed class WorldRenderer : BasicRenderer
    {
        private World _world;

        public WorldRenderer(World world, Square frame, char clearChar = ' ') : base(frame, clearChar)
        {
            _world = world;
        }

        public WorldRenderer(World world, Vector2 size, Vector2 position = default, char clearChar = ' ')
            : this(world, new(size, position), clearChar) { }

        public void BindWorld(World world)
        {
            _world = world;
        }

        public void WorldToBuffer()
        {
            for (int y = 0; y < Size.Y; ++y)
                for (int x = 0; x < Size.X; ++x)
                    _frameBuffer[y * Size.X + x] = _world[x, y].Image;

            foreach (Entity entity in _world.Entities)
                _frameBuffer[entity.Position.Y * Size.X + entity.Position.X] = entity.Image;

            foreach (Projectile projectile in _world.Projectiles)
                _frameBuffer[projectile.Position.Y * Size.X + projectile.Position.X] = projectile.Image;
        }
    }

    sealed class UIRenderer : BasicRenderer
    {
        private string _UIData;

        private char _borderVertical;
        private char _borderHorizontal;
        private char _borderULAngle;
        private char _borderURAngle;
        private char _borderBRAngle;
        private char _borderBLAngle;

        public UIRenderer(Square frame, char clearChar = ' ') : base(frame, clearChar)
        {
            _UIData = "";
            _borderVertical = '?';
            _borderHorizontal = '?';
            _borderULAngle = '?';
            _borderURAngle = '?';
            _borderBRAngle = '?';
            _borderBLAngle = '?';
        }

        public UIRenderer(Vector2 size, Vector2 position = default, char clearChar = ' ')
            : this(new(size, position), clearChar) { }

        public void SetUIData(string data)
        {
            _UIData = data;
        }

        public void AddUIData(string data)
        {
            _UIData += data;
        }

        public void FlushUIData()
        {
            _UIData = "";
        }

        public void SetBorder(char vertical,
                              char horizontal,
                              char topLeftAngle,
                              char topRightAngle,
                              char bottomRightAngle,
                              char bottomLeftAngle)
        {
            _borderVertical = vertical;
            _borderHorizontal = horizontal;
            _borderULAngle = topLeftAngle;
            _borderURAngle = topRightAngle;
            _borderBRAngle = bottomRightAngle;
            _borderBLAngle = bottomLeftAngle;
        }

        public void SetBorder(char vertical, char horizontal, char angle) =>
            SetBorder(vertical, horizontal, angle, angle, angle, angle);

        public void DataToBuffer(bool addBorder)
        {
            int borderOffset = 1;
            int bufferSize = (Size.X - 2) * (Size.Y - 2);

            if (addBorder)
                MakeBorder();
            else
            {
                borderOffset = 0;
                bufferSize = Size.X * Size.Y;
            }

            int Length = Math.Min(bufferSize, _UIData.Length);

            for (int i = 0, y = borderOffset, x = borderOffset; i < Length; ++i)
            {
                if (_UIData[i] == '\n' || x == Size.X - borderOffset)
                {
                    ++y;
                    x = borderOffset;
                }
                if (_UIData[i] == '\n')
                    continue;
                _frameBuffer[y * Size.X + x] = _UIData[i];
                ++x;
            }
        }

        private void MakeBorder()
        {
            int lastRowY = (Size.Y - 1) * Size.X;
            int maxX = Size.X - 1;
            int maxY = Size.Y - 1;

            for (int x = 1; x < maxX; ++x)
                _frameBuffer[x] = _frameBuffer[lastRowY + x] = _borderHorizontal;
            for (int y = 1; y < maxY; ++y)
                _frameBuffer[y * Size.X] = _frameBuffer[(y + 1) * Size.X - 1] = _borderVertical;

            _frameBuffer[0] = _borderULAngle;
            _frameBuffer[maxX] = _borderURAngle;
            _frameBuffer[maxY * Size.X] = _borderBLAngle;
            _frameBuffer[(Size.Y * Size.X) - 1] = _borderBRAngle;
        }
    }
}
