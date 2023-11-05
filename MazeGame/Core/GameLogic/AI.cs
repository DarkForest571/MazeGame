using MazeGame.Core.GameObjects;

namespace MazeGame.Core.GameLogic
{
    interface IAIControlable
    {
        public void UpdateAI(World world, Player player, int framesPerSecond);

        public void AIAction(World world, Player player, int framesPerSecond);
    }
}
