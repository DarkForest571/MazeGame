using MazeGame.Graphics;

namespace MazeGame.Core
{
    abstract class GameObject
    {
        private IImage _image;

        protected GameObject(IImage image)
        {
            _image = image;
        }

        public IImage Image { get => _image; protected set => _image = value; }
    }
}
