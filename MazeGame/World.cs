using System.Text;

namespace MazeGame
{
    class World
    {
        public readonly int MAX_X;
        public readonly int MAX_Y;

        private Tile[,] m_map;

        private bool[,] m_maskOfPassability;
        private int m_countOfPassableTiles;

        private Entitiy[,] m_mapOfEntities;
        private int m_countOfEntities;

        public World(int ySize, int xSize, Image wallImage, Image spaceImage)
        {
            MAX_Y = ySize;
            MAX_X = xSize;
            m_map = new Tile[MAX_Y, MAX_X];
            m_mapOfEntities = new Entitiy[MAX_Y, MAX_X];
            m_maskOfPassability = new bool[MAX_Y, MAX_X];
            m_countOfPassableTiles = 0;
            m_countOfEntities = 0;

            Generate(wallImage, spaceImage);
            RefreshMask();
        }

        public ref readonly Tile[,] GetMap() => ref m_map;

        public void RefreshMask()
        {
            for (int y = 0; y < MAX_Y; ++y)
                for (int x = 0; x < MAX_X; ++x)
                    if (m_map[y, x] is PassableTile)
                    {
                        m_maskOfPassability[y, x] = true;
                        m_countOfPassableTiles++;
                    }
                    else
                        m_maskOfPassability[y, x] = false;
        }

        // Generator

        public void Generate(Image wallImage, Image spaceImage)
        {
            for (int y = 0; y < MAX_Y; ++y)
                m_map[y, 0] = m_map[y, MAX_X - 1] = new Wall(wallImage);
            for (int x = 1, end = MAX_X - 1; x < end; ++x)
                m_map[0, x] = m_map[MAX_Y - 1, x] = new Wall(wallImage);

            for (int y = 1, yend = MAX_Y - 1; y < yend; ++y)
                for (int x = 1, xend = MAX_X - 1; x < xend; ++x)
                    m_map[y, x] = new Space(spaceImage);
        }

        // Entitiy manager

        public bool SpawnPlayer(Player player, Mutex mutex)
        {
            return PlaceRandom(player, mutex);
        }

        private bool PlaceRandom(Entitiy entitiy, Mutex mutex)
        {
            int realCount = m_countOfPassableTiles - m_countOfEntities;

            if (realCount == 0)
                return false;

            Random rnd = new Random();
            int xPos, yPos;
            do
            {
                xPos = rnd.Next(MAX_X);
                yPos = rnd.Next(MAX_Y);
            } while (!m_maskOfPassability[yPos, xPos] || m_mapOfEntities[yPos, xPos] != null);

            entitiy.SetPosition(xPos, yPos);
            mutex.WaitOne();
            m_mapOfEntities[yPos,xPos] = entitiy;
            mutex.ReleaseMutex();
            return true;
        }

        // Renderer

        public void Render(Mutex mutex)
        {
            Console.CursorVisible = false;

            string output = "";

            mutex.WaitOne();
            for (int yCoord = 0; yCoord < MAX_Y; ++yCoord)
            {
                for (int xCoord = 0; xCoord < MAX_X; ++xCoord)
                    if (m_mapOfEntities[yCoord, xCoord] != null)
                        output += m_mapOfEntities[yCoord, xCoord].GetImage().GetData();
                    else
                        output += m_map[yCoord, xCoord].GetImage().GetData();
                output += '\n';
            }
            mutex.ReleaseMutex();

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(output);
            Console.SetCursorPosition(0, MAX_Y);
        }
    }
}
