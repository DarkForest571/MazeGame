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
            set => _frame.Position = value.IsPositive() ? value : new Vector2();
        }
    }

    abstract class SceneRenderer : BasicRenderer
    {
        protected IImage[,] _buffer;

        protected SceneRenderer(Vector2 size, Vector2 position = new Vector2()) : base(size, position)
        {
            _buffer = new IImage[size.X, size.Y];
        }

        public abstract void Render(Tile[,] map, List<Entity> entities);
    }

    class UIRenderer : BasicRenderer
    {
        public UIRenderer(Vector2 size, Vector2 position = new Vector2()) : base(size, position)
        {

        }

    }
}
