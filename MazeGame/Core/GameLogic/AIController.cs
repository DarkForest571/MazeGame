using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    class AIController
    {
        private World _world;

        private Player _player;

        public AIController(World world)
        {
            _world = world;
        }

        public Player Player { get => _player; set => _player = value; }

        public void UpdateAllAI(int framesPerSecond)
        {
            foreach (Entity entity in _world.Entities)
            {
                if (entity is IAIControlable)
                {
                    IAIControlable AIEntity = (IAIControlable)entity;
                    switch (AIEntity.AIState)
                    {
                        case AIState.Idle:
                            AIEntity.TickIdleTimer();
                            if (EntityVisibility(_world, entity, _player, 10)) // TODO 10 is view distance. Put it in AIControlable
                            {
                                AIEntity.AIState = AIState.GettingAttackPosition;
                                AIEntity.TargetPosition = _player.Position;
                            }
                            else if (AIEntity.IdleTimer == 0)
                            {
                                AIEntity.SetNewIdleFrames(framesPerSecond);
                                AIEntity.AIState = AIState.Wander;
                            }
                            break;
                        case AIState.Wander:
                            if (EntityVisibility(_world, entity, _player, 10))
                            {
                                AIEntity.AIState = AIState.GettingAttackPosition;
                                AIEntity.TargetPosition = _player.Position;
                            }
                            else if (AIEntity.TargetPosition == entity.Position)
                            {
                                AIEntity.AIState = AIState.Idle;
                            }
                            break;
                        case AIState.GettingAttackPosition: // Work in progress
                            if (!EntityVisibility(_world, entity, _player, 10))
                            {
                                AIEntity.AIState = AIState.Follow;
                            }
                            else if (AIEntity.IsOnAttackPosition())
                            {
                                AIEntity.AIState = AIState.Attack;
                            }
                            break;
                        case AIState.Attack:
                            if (!EntityVisibility(_world, entity, _player, 10))
                            {
                                AIEntity.AIState = AIState.Follow;
                            }
                            break;
                        case AIState.Follow:
                            if (entity.Position == AIEntity.TargetPosition)
                            {
                                if (EntityVisibility(_world, entity, _player, 10))
                                {
                                    AIEntity.AIState = AIState.GettingAttackPosition;
                                    AIEntity.TargetPosition = _player.Position;
                                }
                                else
                                {
                                    AIEntity.AIState = AIState.Idle;
                                }
                            }
                            break;
                    }
                }
            }
        }

        private bool EntityVisibility(World world, Entity viewer, Entity target, int distance)
        {
            if (Math.Abs(viewer.Position.X - target.Position.X) > distance ||
               Math.Abs(viewer.Position.Y - target.Position.Y) > distance)
                return false;

            Vector2 from, to;
            Direction deltaStep;
            if (viewer.Position.X == target.Position.X)
            {
                (from, to) =
                    target.Position.Y <= target.Position.Y
                    ? (target.Position, target.Position)
                    : (target.Position, target.Position);
                deltaStep = Direction.Down;
            }
            else if (viewer.Position.Y == target.Position.Y)
            {
                (from, to) =
                    target.Position.X <= target.Position.X
                    ? (target.Position, target.Position)
                    : (viewer.Position, target.Position);
                deltaStep = Direction.Right;
            }
            else
                return false;

            bool visible = true;
            for (; from != to; from += deltaStep)
            {
                if (!world[from].IsPassable)
                {
                    visible = false;
                    break;
                }
            }
            return visible;
        }
    }
}
