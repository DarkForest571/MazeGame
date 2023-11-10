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
                    bool canSee = EntityVisibility(_world, entity, _player, AIEntity.ViewDistance);
                    AIEntity.HandleAIState(_world, _player.Position, canSee, framesPerSecond);
                }
            }
        }

        public void ActionAllAI(int framesPerSecond)
        {
            foreach (Entity entity in _world.Entities)
            {
                if (entity is IAIControlable)
                {
                    ((IAIControlable)entity).AIAction(_world, framesPerSecond);
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
                    target.Position.Y <= viewer.Position.Y
                    ? (target.Position, viewer.Position)
                    : (viewer.Position, target.Position);
                deltaStep = Direction.Down;
            }
            else if (viewer.Position.Y == target.Position.Y)
            {
                (from, to) =
                    target.Position.X <= viewer.Position.X
                    ? (target.Position, viewer.Position)
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
