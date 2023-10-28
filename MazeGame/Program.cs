using System.Diagnostics;
using MazeGame;
using MazeGame.Core;

const int MAX_X = 40;
const int MAX_Y = 20;
const int framesPerSecond = 50;
const double msPerFrame = 1000.0 / framesPerSecond;
const long deltaTicks = (long)(msPerFrame * 10000);

Image wallImage = new Image('#');
Image spaceImage = new Image('.');
Image playerImage = new Image('☻');

World world = new World(MAX_X, MAX_Y);
world.Generate(wallImage, spaceImage);

Thread inputThread = new Thread(InputProcessing);
inputThread.Start();

long frame = 0;
long lag;

Stopwatch stopwatch = Stopwatch.StartNew();
while (inputThread.IsAlive)
{
    lag = stopwatch.ElapsedTicks;

    if (lag >= deltaTicks)
        stopwatch.Restart();
    while (lag >= deltaTicks)
    {
        if (frame++ % framesPerSecond == 0)
        {
            //
        }
        Console.SetCursorPosition(0, MAX_Y);
        Console.WriteLine("{0:F3} FPS", ((float)deltaTicks / (float)lag) * framesPerSecond);
        Console.WriteLine("{0,-9} microseconds per frame", lag / 10);
        Console.WriteLine("{0,-20}", new string('|', (int)((frame % framesPerSecond) * 20 / framesPerSecond)));
        Console.WriteLine(frame);
        lag -= deltaTicks;
    }

    world.Render();
}
Console.Clear();

void InputProcessing()
{
    ConsoleKeyInfo key = new ConsoleKeyInfo();

    while (key.Key != ConsoleKey.Escape)
    {
        key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.C:
                Random rnd = Random.Shared;
                Vector2 position = new Vector2();

                do (position.X, position.Y) = (rnd.Next(MAX_X), rnd.Next(MAX_Y));
                while (!world.PlaceEntity(new Player(playerImage, position)));
                break;
            case ConsoleKey.Enter:
                break;
        }
    }
}