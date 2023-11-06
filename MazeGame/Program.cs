using MazeGame.Core;

internal class Program
{
    static void Main(string[] args)
    {
        const int worldWidth = 61;
        const int worldHight = 25;
        const int framesPerSecond = 50;
        
        Game gameInstance = new Game(new (worldWidth, worldHight));

        gameInstance.InitGame();

        gameInstance.RunGameLoop(framesPerSecond);
    }

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
                //run = false;
                break;
        }
    }
}