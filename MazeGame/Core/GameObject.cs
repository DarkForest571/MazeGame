using MazeGame.Graphics;

namespace MazeGame.Core
{
    abstract class GameObject
    {
        private char _image;

        protected GameObject(char image)
        {
            _image = image;
        }

        public char Image { get => _image; protected set => _image = value; }
    }
}
