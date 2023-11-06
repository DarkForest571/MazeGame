﻿using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface IGameController
    {
        public bool ExitCommand { get; }

        public void InitLevel();

        public void AddSpawner(ISpawner spawner);

        public void UpdateAI(int framesPerSecond);

        public void UpdateEntities(int framesPerSecond);

        public bool CheckWinCondition();

        public void HandleInput();
    }

    sealed class MazeGameController : IGameController
    {
        private World _world;
        private ISpawner _playerSpawner;
        private Player _currentPlayer;

        private FinalHatch _finalHatch;
        private Grave _grave;

        private List<ISpawner> _enemySpawners;
        private LinkedList<PlayerCommand> _playerCommands;

        bool _exitCommand;

        public MazeGameController(World world, Player player, FinalHatch finalHatch, Grave grave)
        {
            _world = world;
            _playerSpawner = new WorldwiseSpawner(world, player, 1);

            _finalHatch = finalHatch;
            _grave = grave;

            _enemySpawners = new List<ISpawner>();
            _playerCommands = new LinkedList<PlayerCommand>();

            _exitCommand = false;
        }

        public bool ExitCommand => _exitCommand;

        public void InitLevel()
        {
            _world.RemoveAllCreatures();

            Vector2 position = _world.GetRandomPositionByCondition((tile) => tile.IsPassable());
            _world[position] = _finalHatch;

            _currentPlayer = (Player)_playerSpawner.SpawnOne();

            foreach (ISpawner spawner in _enemySpawners)
                spawner.SpawnAll();
        }

        public void AddSpawner(ISpawner spawner)
        {
            _enemySpawners.Add(spawner);
        }

        public void UpdateAI(int framesPerSecond)
        {
            foreach (Creature creature in _world.Creatures)
            {
                if (creature is IAIControlable)
                    ((IAIControlable)creature).UpdateAI(_world, _currentPlayer, framesPerSecond);
            }
        }

        public void UpdateEntities(int framesPerSecond)
        {
            foreach (Creature creature in _world.Creatures)
            {
                creature.UpdateMoveTimer();

                if (creature is IAIControlable)
                    ((IAIControlable)creature).AIAction(_world, _currentPlayer, framesPerSecond);

                if (creature is Player)
                {
                    foreach (PlayerCommand command in _playerCommands)
                    {
                        switch (command)
                        {
                            case PlayerCommand.GoUp:
                            case PlayerCommand.GoRight:
                            case PlayerCommand.GoDown:
                            case PlayerCommand.GoLeft:
                                Direction directionToMove = command switch
                                {
                                    PlayerCommand.GoUp => Direction.Up,
                                    PlayerCommand.GoRight => Direction.Right,
                                    PlayerCommand.GoDown => Direction.Down,
                                    PlayerCommand.GoLeft => Direction.Left
                                };
                                List<Direction> availableDirections = 
                                    _world.GetNeighborsByCondition(_currentPlayer.Position, (tile) => tile.IsPassable());
                                if (availableDirections.Contains(directionToMove))
                                {
                                    _currentPlayer.MoveTo(directionToMove,
                                        _world[_currentPlayer.Position + directionToMove].MoveCost);
                                }
                        break;
                            case PlayerCommand.Attack:
                                break;
                        }
                    }
                    _playerCommands.Clear();
                }
            }
        }

        public bool CheckWinCondition()
        {
            return _world[_currentPlayer.Position] is FinalHatch;
        }

        public void HandleInput()
        {

            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.W:
                        _playerCommands.AddLast(PlayerCommand.GoUp);
                        break;
                    case ConsoleKey.A:
                        _playerCommands.AddLast(PlayerCommand.GoLeft);
                        break;
                    case ConsoleKey.S:
                        _playerCommands.AddLast(PlayerCommand.GoDown);
                        break;
                    case ConsoleKey.D:
                        _playerCommands.AddLast(PlayerCommand.GoRight);
                        break;
                    case ConsoleKey.Spacebar:
                        _playerCommands.AddLast(PlayerCommand.Attack);
                        break;
                    case ConsoleKey.Escape:
                        _exitCommand = true;
                        break;
                }
            }
        }
    }
}
