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
        private AIController _AIcontroller;

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
            Tile space = new Tile(' ', true, 0.5f);
            _finalHatch = new Tile('#', true, 1.0f);
            _grave = new Tile('†', true, 0.25f);

            //_generator = new MazeGenerator(_world, wall, space);
            _generator = new DefaultGenerator(_world, wall, space);

            // Entities, projectiles, entities and spawners
            Projectile meleeAttack = new Projectile('\\', 1, 0.1f, 15);
            Projectile horizontalRangeAttack = new Projectile('-', 10, 0.2f, 10);
            Projectile verticalRangeAttack = new Projectile('|', 10, 0.2f, 10);
            Player player = new Player('☻', meleeAttack, 100, 10f, 0.01f);
            Zombie zombie = new Zombie('Z', meleeAttack, 50, 1f, 10);
            Shooter shooter = new Shooter('S', horizontalRangeAttack, verticalRangeAttack, 25, 2f, 10);

            _playerSpawner = new WorldwiseSpawner(_world, player, 1);
            _enemySpawners = new List<ISpawner>
            {
                new WorldwiseSpawner(_world, zombie, 15),
                new WorldwiseSpawner(_world, shooter, 10)
            };
            _AIcontroller = new AIController(_world);

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

            _world.ClearAllGameObjects();
            _currentPlayer = (Player)_playerSpawner.SpawnOne();
            _AIcontroller.Player = _currentPlayer;
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

            int level = 1;

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

                    // HUD
                    _UIRenderer.SetUIData($"Health: {_currentPlayer.Health}\nLevel: {level}\n");

                    // Logs
                    float FPS = deltaMicroseconds / (float)lag * _framesPerSecond;
                    int barLength = (int)(frame % _framesPerSecond * 20 / _framesPerSecond);
                    if (barLength == 0)
                        for (int i = 0; i < 20; i++)
                            bar[i] = ' ';
                    else
                        bar[barLength - 1] = '|';

                    _UIRenderer.AddUIData($"{FPS,6:F3} FPS\n{(lag / 10),6:D} mics/frame\n" + new string(bar));

                    // Render
                    RenderScene();

                    // Win and Game Over check
                    if (CheckWinCondition())
                    {
                        RestartLevel();
                        level++;
                    }
                    if (CheckGameOverCondition())
                    {
                        RestartLevel();
                        level = 1;
                    }

                    ++frame;
                }
            }

            Console.Clear();
        }

        private void UpdateScene()
        {
            _AIcontroller.UpdateAllAI(_framesPerSecond);
            _AIcontroller.ActionAllAI(_framesPerSecond);
            _world.UpdateProjectiles(_framesPerSecond);
            _currentPlayer.Update(_world, _inputHandler, _framesPerSecond);

            _world.ClearDeadEntities();
            _world.ClearDeadProjectile();
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

        private bool CheckWinCondition() =>
            _currentPlayer.Position == _finalHatchPosition;

        private bool CheckGameOverCondition() =>
            _currentPlayer.Health <= 0;
    }
}
