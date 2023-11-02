namespace MazeGame.Utils
{
    struct Square
    {
        public Vector2 Position;
        public Vector2 Size;

        public Square(Vector2 size, Vector2 position = new Vector2())
        {
            Position = position;
            Size = size;
        }
    }
}
