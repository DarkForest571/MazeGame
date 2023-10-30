namespace MazeGame.Graphics
{
    interface Image { }

    class CharImage<Type> : Image
    {
        private char _value;

        public CharImage(char value)
        {
            _value = value;
        }

        public char Data { get => _value; set => _value = value; }
    }
}
