using System.Diagnostics;
using MazeGame.Core;
using MazeGame.Graphics;
using MazeGame;

internal class Program
{
    static bool run = true;

    static void Main(string[] args)
    {
        const int MAX_X = 40;
        const int MAX_Y = 20;
        const int framesPerSecond = 50;
        const double msPerFrame = 1000.0 / framesPerSecond;
        const long deltaMicroseconds = (long)(msPerFrame * 10000);

        IImage wallImage = new Image<char>('#');
        IImage spaceImage = new Image<char>('.');
        IImage playerImage = new Image<char>('☻');

        Wall wallTile = new Wall(wallImage);
        Space spaceTile = new Space(spaceImage);

        Generator generator = new MazeGenerator(wallTile,spaceTile);

        Game gameInstance = new Game(new Vector2(MAX_X, MAX_Y), generator);
        gameInstance.CreateWorld();

        long frame = 0;
        long lag;

        //InputProcessing();
        Mutex m;

        Stopwatch stopwatch = Stopwatch.StartNew();
        while (run)
        {

            // InputProcessing

            lag = stopwatch.ElapsedTicks;

            if (lag >= deltaMicroseconds)
            {
                stopwatch.Restart();
                if (frame++ % framesPerSecond == 0)
                {
                    //
                }

                // Physics


                // Logs
                Console.SetCursorPosition(0, MAX_Y);
                Console.WriteLine("{0:F3} FPS", deltaMicroseconds / (float)lag * framesPerSecond);
                Console.WriteLine("{0,-9} mics/frame", lag / 10);
                Console.WriteLine("{0,-20}", new string('|', (int)(frame % framesPerSecond * 20 / framesPerSecond)));

                // Render
                gameInstance.RenderScene();
            }
        }
        Console.Clear();



        async void InputProcessing()
        {
            ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();
            await Task.Run(() => { consoleKeyInfo = Console.ReadKey(true); });

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.C:
                    //Random rnd = Random.Shared;
                    //Vector2 position = new Vector2();

                    //do (position.X, position.Y) = (rnd.Next(MAX_X), rnd.Next(MAX_Y));
                    //while (!world.PlaceEntity(new Player(playerImage, position)));
                    break;
                case ConsoleKey.Escape:
                    run = false;
                    break;
            }
        }
    }
}