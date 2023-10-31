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

    class CharWorldRenderer : WorldRenderer
    {
        private char[] _stringBuffer;
        private char _errorChar;

        public CharWorldRenderer(Vector2 size, Vector2 position = new Vector2(), char errorChar = '?') : base(size, position)
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

    class UIRenderer : BasicRenderer
    {
        public UIRenderer(Vector2 size, Vector2 position = new Vector2()) : base(size, position)
        {

        }

    }
}
