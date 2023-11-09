using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    enum AIState
    {
        Idle,
        Wander,
        Follow,
        Flee,
        Attack
    }

    interface IAIControlable
    {
        public int IdleTimer { get; }

        public void TickIdleTimer();

        public void SetNewIdleFrames(int framesPerSecond);

        public AIState AIState { get; set; }

        public Vector2 TargetPosition { get; set; }

        public void UpdateAI(World world, Player player, int framesPerSecond);

        public void AIAction(World world, Player player, int framesPerSecond);
    }
}
