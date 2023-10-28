namespace MazeGame
{
    class Image
    {
        private char m_value;

        private ConsoleColor m_BackgroundColor;
        private ConsoleColor m_ForegroundColor;

        public Image(char value = '?',
            ConsoleColor backgroundColor = ConsoleColor.Black,
            ConsoleColor charColor = ConsoleColor.White)
        {
            m_value = value;
            m_BackgroundColor = backgroundColor;
            m_ForegroundColor = charColor;
        }

        public void SwapData(Image image)
        {
            (image.m_value, m_value) = (m_value, image.m_value);
        }

        public ref readonly char GetData() => ref m_value;

        public ConsoleColor GetBackgroundColor() => m_BackgroundColor;

        public ConsoleColor GetForegroundColor() => m_ForegroundColor;

        public void SwapColors()
        {
            (m_BackgroundColor, m_ForegroundColor) = (m_ForegroundColor, m_BackgroundColor);
        }
    }
}
