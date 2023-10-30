namespace MazeGame.Graphics
{
    interface IImage { }

    class Image<T> : IImage
    {
        private T _value;

        public Image(T value)
        {
            _value = value;
        }

        public T Data { get => _value; set => _value = value; }
    }
}
