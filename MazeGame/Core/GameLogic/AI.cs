using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    enum AIState
    {
        Idle,
        Wander,
        GettingAttackPosition,
        Follow,
        Attack
    }

    interface IAIControlable
    {
        public int IdleTimer { get; }

        public void TickIdleTimer();

        public void SetNewIdleFrames(int framesPerSecond);

        public AIState AIState { get; set; }

        public Vector2 TargetPosition { get; set; }

        public bool IsOnAttackPosition();

        public void SetWanderingPosition(World world);

        public void AIAction(World world, Player player, int framesPerSecond);
    }
}
