namespace MazeGame.Core.GameLogic
{
    interface GameController
    {
        public void InitLevel();

        public void UpdateAI();

        public void UpdateEntities();
    }

    sealed class MazeGameController : GameController
    {
        private World _world;

        private List<Spawner> _enemySpawners;

        public MazeGameController(World world)
        {
            _world = world;
            _enemySpawners = new List<Spawner>();
        }

        public void AddSpawner(Spawner spawner)
        {
            _enemySpawners.Add(spawner);
        }

        public void InitLevel()
        {
            foreach (Spawner spawner in _enemySpawners)
            {
                spawner.SpawnAll();
            }
        }

        public void UpdateAI()
        {

        }

        public void UpdateEntities()
        {

        }
    }
}
