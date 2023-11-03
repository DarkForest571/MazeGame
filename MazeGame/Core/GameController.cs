using MazeGame.Core.GameObjects;

namespace MazeGame.Core
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
            foreach (var spawnSettings in _enemiesSpawnSettings)
            {
                SpawnEntities(spawnSettings.Item1, spawnSettings.Item2);
            }
        }

        private void SpawnEntities(Entity entity, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnInRandomPosition(entity);
            }
        }

        private void SpawnInRandomPosition(Entity entity)
        {
            
        }

        public void UpdateAI()
        {

        }

        public void UpdateEntities()
        {

        }
    }
}
