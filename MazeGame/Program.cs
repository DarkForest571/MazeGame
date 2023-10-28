using System.Diagnostics;
using System.Threading;
using MazeGame;

const int MAX_X = 40;
const int MAX_Y = 20;
const int framesPerSecond = 50;
const double msPerFrame = 1000.0 / framesPerSecond;

Image wallImage = new Image('#');
Image spaceImage = new Image('.');
Image playerImage = new Image('☻');

Mutex mutex = new Mutex(false);

World world = new World(MAX_Y, MAX_X, wallImage, spaceImage);

Thread inputThread = new Thread(InputProcessing);
inputThread.Start();

int frame = 0;
long timeBuffer = 0, timeSnap;

const long deltaTicks = (long)(msPerFrame * 10000);
Stopwatch stopwatch = Stopwatch.StartNew();
while (inputThread.IsAlive)
{
    timeSnap = stopwatch.ElapsedTicks;
    if (timeBuffer + timeSnap >= deltaTicks)
    {
        stopwatch.Restart();

        if (frame++ % framesPerSecond == 0)
        {
            wallImage.SwapData(spaceImage);
        }

        world.Render(mutex);

        Console.WriteLine("{0:F3} FPS", ((float)deltaTicks / (float)(timeBuffer + timeSnap)) * framesPerSecond);
        Console.WriteLine("{0,-9} microseconds per frame", (timeBuffer + timeSnap) / 10);
        Console.WriteLine("{0,-20}", new string('|', (frame % framesPerSecond) * 20 / framesPerSecond));
        Console.WriteLine("{0,9}", timeBuffer);
        Console.WriteLine(frame);
        timeBuffer = timeBuffer + timeSnap - deltaTicks;
    }
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
                world.SpawnPlayer(new Player(playerImage), mutex);
                break;
            case ConsoleKey.Enter:
                break;
        }
    }
}