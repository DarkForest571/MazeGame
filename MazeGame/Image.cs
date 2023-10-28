namespace MazeGame
{
    class Image
    {
        private char _value;

        public Image(char value = '?')
        {
            _value = value;
        }

        public char Data { get => _value; set => _value = value; }
    }
}
