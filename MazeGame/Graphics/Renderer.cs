using MazeGame.Core;

namespace MazeGame.Graphics
{
    abstract class BasicRenderer
    {
        private Square _frame;

        protected BasicRenderer(Vector2 size, Vector2 position = new Vector2())
        {
            _frame = new Square(size, position);
        }

        public Vector2 Size { get => _frame.Size; }

        public Vector2 Position
        {
            get => _frame.Position;
            set => _frame.Position = value > Vector2.Zero ? value : new Vector2();
        }
    }

    abstract class WorldRenderer : BasicRenderer
    {
        protected IImage[,] _buffer;

        protected WorldRenderer(Vector2 size, Vector2 position = new Vector2()) : base(size, position)
        {
            _buffer = new IImage[size.X, size.Y];
        }

        public abstract void Render(World world);

        public void FillBuffer(World world)
        {
            for (int y = 0; y < Size.Y; ++y)
            {
                for (int x = 0; x < Size.X; ++x)
                {
                    _buffer[x, y] = world.GetTile(x, y).Image;
                }
            }

            for (int i = 0; i < world.EntityCount(); i++)
            {
                Entity entity = world.GetEntity(i);
                _buffer[entity.Position.X, entity.Position.Y] = entity.Image;
            }
        }
    }

    sealed class ConsoleWorldRenderer : WorldRenderer
    {
        private char[] _stringBuffer;
        private char _errorChar;

        public ConsoleWorldRenderer(Vector2 size, Vector2 position = new Vector2(), char errorChar = '?') : base(size, position)
        {
            _stringBuffer = new char[size.X];
            _errorChar = errorChar;
        }

        public override void Render(World world)
        {
            FillBuffer(world);

            Console.CursorVisible = false;
            for (int y = 0; y < Size.Y; ++y)
            {
                Console.SetCursorPosition(Position.X, Position.Y + y);
                for (int x = 0; x < Size.X; ++x)
                {
                    Image<char>? image = _buffer[x, y] as Image<char>;
                    if (image != null)
                        _stringBuffer[x] = image.Data;
                    else
                        _stringBuffer[x] = _errorChar;
                }
                Console.Write(_stringBuffer);
            }
        }
    }

    abstract class UIRenderer : BasicRenderer
    {
        protected UIRenderer(Vector2 size, Vector2 position = new Vector2()) : base(size, position) { }

        public abstract void Render(string data);
    }

    class ConsoleUIRenderer : UIRenderer
    {
        char[] _frameBuffer;

        public ConsoleUIRenderer(Vector2 size, Vector2 position = new Vector2()) : base(size, position)
        {
            _frameBuffer = new char[size.X * size.Y];

            int lastRowY = (size.Y - 1) * size.X;
            int maxX = size.X - 1;
            int maxY = size.Y - 1;

            for (int x = 1; x < maxX; ++x)
                _frameBuffer[x] = _frameBuffer[lastRowY + x] = '-';
            for (int y = 1; y < maxY; ++y)
                _frameBuffer[y * Size.X] = _frameBuffer[(y + 1) * Size.X - 1] = '|';

            _frameBuffer[0] = _frameBuffer[maxX] = _frameBuffer[maxY * Size.X] = _frameBuffer[(Size.Y * Size.X) - 1] = '+';

            Clear();
        }

        private void Clear()
        {
            for (int y = 0; y < Size.Y - 2; ++y)
                for (int x = 0; x < Size.X - 2; ++x)
                    ContentAt(x, y) = ' ';
        }

        private ref char ContentAt(int x, int y)
        {
            return ref _frameBuffer[(y + 1) * Size.X + x + 1];
        }

        public override void Render(string data)
        {
            Clear();

            int bufferSize = (Size.X - 2) * (Size.Y - 2);
            int Length = Math.Min(bufferSize, data.Length);

            for (int i = 0, y = 0, x = 0; i < Length; ++i)
            {
                ContentAt(x, y) = data[i];
                ++x;
                if (data[i] == '\n' || x == Size.X - 2)
                {
                    ++y;
                    x = 0;
                }
            }

            for (int y = 0; y < Size.Y; ++y)
            {
                Console.SetCursorPosition(Position.X, Position.Y + y);
                Console.Write(new string(_frameBuffer, y * Size.X, Size.X));
            }
        }
    }
}
