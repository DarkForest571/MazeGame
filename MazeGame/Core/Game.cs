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
        private IGameController _controller;

        private WorldRenderer _worldRenderer;
        private UIRenderer _UIRenderer;

        public Game(Vector2 worldSize)
        {
            _world = new World(worldSize);
            //_generator = new MazeGenerator(_world, new Wall('█'), new Space(' '));
            _generator = new DefaultGenerator(_world, new Wall('█'), new Space(' '));

            _controller = new MazeGameController(_world,
                                                 new Player('☻'),
                                                 new FinalHatch('#'),
                                                 new Grave('†'));

            _worldRenderer = new WorldRenderer(_world, worldSize);
            _UIRenderer = new UIRenderer(new(21, 8), new Vector2(worldSize.X, 0));
        }

        public void InitGame()
        {
            ISpawner zombieSpawner = new WorldwiseSpawner(_world, new Zombie('Z'), 50);
            ISpawner shooterSpawner = new WorldwiseSpawner(_world, new Shooter('S'), 10);

            _controller.AddSpawner(zombieSpawner);
            _controller.AddSpawner(shooterSpawner);

            _UIRenderer.SetBorder('║', '═', '╔', '╗', '╝', '╚');

            RestartLevel();
        }

        private void RestartLevel()
        {
            _generator.Generate();
            _controller.InitLevel();
        }

        public void RunGameLoop(int framesPerSecond)
        {
            double msPerFrame = 1000.0 / framesPerSecond;
            long deltaMicroseconds = (long)(msPerFrame * 10000);

            bool run = true;
            char[] bar = new char[20];

            long frame = 0;
            long lag = -1;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (run)
            {

                // InputProcessing

                lag = stopwatch.ElapsedTicks;

                if (lag >= deltaMicroseconds)
                {
                    stopwatch.Restart();
                    if (frame % framesPerSecond == 0)
                    {
                        //
                    }

                    // Game logic

                    _controller.UpdateAI(framesPerSecond);
                    _controller.UpdateEntities(framesPerSecond);

                    // Game logic end
                    // Logs

                    float FPS = deltaMicroseconds / (float)lag * framesPerSecond;
                    int barLength = (int)(frame % framesPerSecond * 20 / framesPerSecond);
                    if (barLength == 0)
                        for (int i = 0; i < 20; i++)
                            bar[i] = ' ';
                    else
                        bar[barLength - 1] = '|';

                    _UIRenderer.SetUIData($"{FPS,6:F3} FPS\n{(lag / 10),6:D} mics/frame\n" + new string(bar));

                    // Logs end
                    // Render

                    RenderScene();

                    // Render end

                    ++frame;
                }
            }
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
    }
}
