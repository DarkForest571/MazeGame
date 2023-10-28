namespace MazeGame
{
    abstract class Entitiy
    {
        private Image m_entitiyImage;

        private int m_x, m_y;
        private int m_health;
        private float m_moveCoefficient;

        public Entitiy(Image entitiyImage, int xPosition, int yPosition, int health, float moveCoefficient)
        {
            m_entitiyImage = entitiyImage;
            m_x = xPosition;
            m_y = yPosition;
            m_health = health;
            m_moveCoefficient = moveCoefficient;
        }

        public Image GetImage() => m_entitiyImage;

        public int GetX() => m_x;

        public int GetY() => m_y;

        public void SetPosition(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public int HitEntity(int damage) => m_health -= damage;

        public float GetMoveCoefficient() => m_moveCoefficient;
    }

    class Player : Entitiy
    {
        public Player(Image entitiyImage, int xPosition = 0, int yPosition = 0, int health = 100, float moveCoefficient = 1.0f)
        : base(entitiyImage, xPosition, yPosition, health, moveCoefficient) { }
    }
}
