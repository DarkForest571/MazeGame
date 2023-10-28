namespace MazeGame
{
    class Image
    {
        private char m_value;

        public Image(char value = '?')
        {
            m_value = value;
        }

        public void SwapData(Image image)
        {
            (image.m_value, m_value) = (m_value, image.m_value);
        }

        public ref readonly char GetData() => ref m_value;
    }
}
