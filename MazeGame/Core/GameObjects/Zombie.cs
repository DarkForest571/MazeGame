using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Zombie : Creature, IAIControlable
    {
        private char _attackImage;

        private int _idleFramesTimer;
        private int _idleSecondsFrom;
        private int _idleSecondsTo;

        private AIState _AIstate;

        private Vector2 _targetPosition;

        public Zombie(char zombieImage,
                      char attackImage,
                      Vector2 position = default,
                      int health = 150,
                      float moveSpeed = 0.25f) : base(zombieImage,
                                                           position,
                                                           health,
                                                           moveSpeed)
        {
            _attackImage = attackImage;

            _idleFramesTimer = 0;
            _idleSecondsFrom = 1;
            _idleSecondsTo = 3;

            _AIstate = AIState.Idle;

            _targetPosition = position;
        }

        public override Zombie Clone() => new Zombie(Image, _attackImage, Position, Health, _moveCoefficient);

        public void UpdateAI(World world, Player player, int framesPerSecond)
        {
            switch (_AIstate)
            {
                case AIState.Idle:
                    if (CheckPlayerVisibility(world, player))
                    {
                        _AIstate = AIState.Follow;
                        _targetPosition = player.Position;
                    }
                    else
                    {
                        _idleFramesTimer--;
                        if (_idleFramesTimer > 0)
                            return;
                        SetNewIdleFrames(framesPerSecond);

                        List<Direction> list = world.GetNeighborsByCondition(Position, (tile) => tile.IsPassable);
                        if (list.Count > 0)
                        {
                            int choise = Random.Shared.Next(list.Count);
                            _targetPosition = Position + list[choise];
                        }
                    }
                    break;
                case AIState.Follow:
                    if (_targetPosition == player.Position &&
                        Vector2.SqDistance(Position, _targetPosition) == 1)
                    {
                        _AIstate = AIState.Attack;
                    }
                    else if (Position == _targetPosition)
                    {
                        _AIstate = AIState.Idle;
                    }
                    break;
                case AIState.Attack:
                    break;
            }

        }

        public void AIAction(World world, Player player, int framesPerSecond)
        {
            switch (_AIstate)
            {
                case AIState.Idle:
                case AIState.Follow:
                    if (MoveTimer == 0)
                    {
                        Direction moveDirection = Vector2.GetDirection(Position, _targetPosition);
                        MoveTo(moveDirection, world[Position + moveDirection].MoveCost);
                    }
                    break;
                case AIState.Attack:
                    break;
            }
        }

        private bool CheckPlayerVisibility(World world, Player player)
        {
            bool visible = false;
            if (player.Position.X == Position.X)
            {
                visible = true;
                int min = Math.Min(player.Position.Y, Position.Y);
                int max = Math.Max(player.Position.Y, Position.Y);
                for (; min < max; min++)
                {
                    if (!world[Position.X, min].IsPassable)
                    {
                        visible = false;
                        break;
                    }
                }
            }
            else if (player.Position.Y == Position.Y)
            {
                visible = true;
                int min = Math.Min(player.Position.X, Position.X);
                int max = Math.Max(player.Position.X, Position.X);
                for (; min < max; min++)
                {
                    if (!world[min, Position.Y].IsPassable)
                    {
                        visible = false;
                        break;
                    }
                }
            }
            return visible;
        }

        private void SetNewIdleFrames(int framesPerSecond)
        {
            _idleFramesTimer = Random.Shared.Next(_idleSecondsFrom * framesPerSecond, _idleSecondsTo * framesPerSecond);
        }

        public override Projectile GetAttackProjectile() =>
            new Projectile(_attackImage, Position + AttackDirection, AttackDirection);
    }
}
