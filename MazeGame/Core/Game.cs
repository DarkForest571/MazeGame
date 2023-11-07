using System.Diagnostics;

using MazeGame.Core.GameLogic;
using MazeGame.Core.GameObjects;
using MazeGame.Graphics;
using MazeGame.Utils;

namespace MazeGame.Core
{
    sealed class Game
    {
        private World _world;
        private IGenerator _generator;

        private ISpawner _playerSpawner;
        private List<ISpawner> _enemySpawners;

        private Vector2 _finalHatchPosition;
        private Tile _finalHatch;
        private Tile _grave;

        private Player _currentPlayer;
        private IInputHandler _inputHandler;

        private WorldRenderer _worldRenderer;
        private UIRenderer _UIRenderer;

        private int _framesPerSecond;

        public Game(Vector2 worldSize, int framesPerSecond)
        {
            // World, tiles and generator
            _world = new World(worldSize);

            Tile wall = new Tile('█', false);
            Tile space = new Tile(' ', true, (int)(0.1 * framesPerSecond));
            _finalHatch = new Tile('#', true, (int)(1.0 * framesPerSecond));
            _grave = new Tile('†', true, (int)(0.25 * framesPerSecond));

            _generator = new MazeGenerator(_world, wall, space);
            //_generator = new DefaultGenerator(_world, wall, space);

            // Entities, projectiles and spawners
            Projectile meleeAttack = new Projectile('/');
            Projectile horizontalRangeAttack = new Projectile('/');
            Projectile verticalRangeAttack = new Projectile('/');
            _playerSpawner = new WorldwiseSpawner(_world, new Player('☻', meleeAttack), 1);
            _enemySpawners = new List<ISpawner>
            {
                new WorldwiseSpawner(_world, new Zombie('Z', meleeAttack), 15),
                new WorldwiseSpawner(_world, new Shooter('S', horizontalRangeAttack, verticalRangeAttack), 10)
            };

            // Input and UI
            _inputHandler = new DefaultInputHandler();

            _worldRenderer = new WorldRenderer(_world, worldSize);
            _UIRenderer = new UIRenderer(new(21, 8), new Vector2(worldSize.X, 0));
            _UIRenderer.SetBorder('║', '═', '╔', '╗', '╝', '╚');

            _framesPerSecond = framesPerSecond;
        }

        public void InitGame()
        {
            RestartLevel();
        }

        private void RestartLevel()
        {
            _generator.Generate();
            _finalHatchPosition = _world.GetRandomPositionByCondition((tile) => tile.IsPassable);
            _world[_finalHatchPosition] = _finalHatch;

            _world.ClearAllEntities();
            _currentPlayer = (Player)_playerSpawner.SpawnOne();
            foreach (ISpawner spawner in _enemySpawners)
                spawner.SpawnAll();
        }

        public void RunGameLoop()
        {
            double msPerFrame = 1000.0 / _framesPerSecond;
            long deltaMicroseconds = (long)(msPerFrame * 10000);

            char[] bar = new char[20];

            long frame = 0;
            long lag = -1;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!_inputHandler.ExitCommand)
            {
                // Input processing
                _inputHandler.PullInput();

                lag = stopwatch.ElapsedTicks;
                if (lag >= deltaMicroseconds)
                {
                    stopwatch.Restart();

                    // Game logic
                    UpdateScene();

                    // Logs
                    float FPS = deltaMicroseconds / (float)lag * _framesPerSecond;
                    int barLength = (int)(frame % _framesPerSecond * 20 / _framesPerSecond);
                    if (barLength == 0)
                        for (int i = 0; i < 20; i++)
                            bar[i] = ' ';
                    else
                        bar[barLength - 1] = '|';

                    _UIRenderer.SetUIData($"{FPS,6:F3} FPS\n{(lag / 10),6:D} mics/frame\n" + new string(bar));

                    // Render
                    RenderScene();

                    if(CheckWinCondition())
                        RestartLevel();

                    ++frame;
                }
            }

            Console.Clear();
        }

        private void UpdateScene()
        {
            UpdateAI(_framesPerSecond);
            UpdateEntities(_framesPerSecond);
        }

        private void RenderScene()
        {
            Console.CursorVisible = false;
            _worldRenderer.ClearBuffer();
            _worldRenderer.WorldToBuffer();
            _worldRenderer.Render();

            _UIRenderer.ClearBuffer();
            _UIRenderer.DataToBuffer(true);
            _UIRenderer.Render();
        }

        private bool CheckWinCondition()
        {
            return _currentPlayer.Position == _finalHatchPosition;
        }

        private void UpdateAI(int framesPerSecond)
        {
            foreach (Entity entity in _world.Entities)
            {
                if (entity is IAIControlable)
                    ((IAIControlable)entity).UpdateAI(_world, _currentPlayer, framesPerSecond);
            }
        }

        private void UpdateEntities(int framesPerSecond)
        {
            foreach (Entity entity in _world.Entities)
            {
                entity.UpdateMoveTimer();

                if (entity is IAIControlable)
                    ((IAIControlable)entity).AIAction(_world, _currentPlayer, framesPerSecond);

                if (entity is Player)
                {
                    foreach (PlayerCommand command in _inputHandler.Commands)
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
                                    _world.GetNeighborsByCondition(_currentPlayer.Position, (tile) => tile.IsPassable);
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
                    _inputHandler.ClearCommands();
                }
            }
        }
    }
}
