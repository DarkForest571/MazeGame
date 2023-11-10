using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface IAIControlable
    {
        public void HandleAIState(World world, Vector2 playerPosition, bool canSeePlayer, int framesPerSecond);

        public void AIAction(World world, int framesPerSecond);
    }
}
