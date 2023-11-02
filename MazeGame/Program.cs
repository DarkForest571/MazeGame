﻿using MazeGame.Core;
using MazeGame.Utils;
using MazeGame;

internal class Program
{
    static bool run = true;

    static void Main(string[] args)
    {
        const int worldWidth = 61;
        const int worldHight = 25;
        const int framesPerSecond = 50;
        
        Wall wallTile = new Wall('█');
        Space spaceTile = new Space(' ');

        Game gameInstance = new Game(new Vector2(worldWidth, worldHight), new MazeGenerator(wallTile, spaceTile));

        gameInstance.Init();

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
                run = false;
                break;
        }
    }
}