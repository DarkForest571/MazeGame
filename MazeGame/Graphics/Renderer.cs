using MazeGame.Core;

namespace MazeGame.Graphics
{
    abstract class BasicRenderer
    {
        protected readonly int _width;
        protected readonly int _height;

        protected BasicRenderer(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public abstract void Render();
    }

    class SceneRenderer : BasicRenderer
    {
        private Image[,] _buffer;

        public SceneRenderer(int width, int height) : base(width, height)
        {
            _buffer = new Image[height, height];
        }

        public override void Render()
        {

        }

        private void Render(Tile[,] map, List<Entity> entities)
        {

        }
    }
}
