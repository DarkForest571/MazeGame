using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface GameController
    {
        public void InitGame();

        public void AddSpawner(Spawner spawner);

        public void UpdateAI();

        public void UpdateEntities();

        public bool CheckWinCondition();
    }

    sealed class MazeGameController : GameController
    {
        private World _world;

        private Player _player;
        private FinalHatch _finalHatch;
        private Grave _grave;

        private List<Spawner> _enemySpawners;

        public MazeGameController(World world, Player player, FinalHatch finalHatch, Grave grave)
        {
            _world = world;
            _player = player;
            _finalHatch = finalHatch;
            _grave = grave;
            _enemySpawners = new List<Spawner>();
        }

        public void InitGame()
        {
            Vector2 position = _world.GetRandomTileByCondition((tile) => tile is PassableTile);
            _world[position] = _finalHatch;

            position = _world.GetRandomTileByCondition((tile) => tile is PassableTile);

            foreach (Spawner spawner in _enemySpawners)
                spawner.SpawnAll();
        }

        public void AddSpawner(Spawner spawner)
        {
            _enemySpawners.Add(spawner);
        }

        public void UpdateAI()
        {

        }

        public void UpdateEntities()
        {

        }

        public bool CheckWinCondition()
        {
            return _world[_player.Position] is FinalHatch;
        }
    }
}
