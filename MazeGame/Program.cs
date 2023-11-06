using MazeGame.Core;

internal class Program
{
    static void Main(string[] args)
    {
        const int worldWidth = 61;
        const int worldHight = 25;
        const int framesPerSecond = 50;
        
        Game gameInstance = new Game(new (worldWidth, worldHight), framesPerSecond);

        gameInstance.InitGame();

        gameInstance.RunGameLoop(framesPerSecond);
    }
}