﻿using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface IGameController
    {
        public void InitLevel();

        public void AddSpawner(ISpawner spawner);

        public void UpdateAI();

        public void UpdateEntities();

        public bool CheckWinCondition();
    }

    sealed class MazeGameController : IGameController
    {
        private World _world;

        private FinalHatch _finalHatch;
        private Grave _grave;

        private ISpawner _playerSpawner;
        private Player _currentPlayer;

        private List<ISpawner> _enemySpawners;

        public MazeGameController(World world, Player player, FinalHatch finalHatch, Grave grave)
        {
            _world = world;
            _playerSpawner = new WorldwiseSpawner(world, player, 1);
            _finalHatch = finalHatch;
            _grave = grave;
            _enemySpawners = new List<ISpawner>();
        }

        public void InitLevel()
        {
            Vector2 position = _world.GetRandomTileByCondition((tile) => tile is PassableTile);
            _world[position] = _finalHatch;

            _currentPlayer = (Player)_playerSpawner.SpawnOne();

            foreach (ISpawner spawner in _enemySpawners)
                spawner.SpawnAll();
        }

        public void AddSpawner(ISpawner spawner)
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
            return _world[_currentPlayer.Position] is FinalHatch;
        }
    }
}