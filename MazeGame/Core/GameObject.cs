using MazeGame.Graphics;

namespace MazeGame.Core
{
    abstract class GameObject
    {
        private Image _image;

        protected GameObject(Image image)
        {
            _image = image;
        }

        public Image Image { get => _image; protected set => _image = value; }
    }
}
