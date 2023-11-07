using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Shooter : Entity, IAIControlable
    {
        Projectile _horizontalAttackProjectile;
        Projectile _verticalAttackProjectile;
        private Direction _attackDirection;
        private int _attackTimer;

        private int _shootDistance;

        private int _idleFramesTimer;
        private int _idleSecondsFrom;
        private int _idleSecondsTo;

        private AIState _AIstate;

        private Vector2 _targetPosition;

        public Shooter(char image,
                      Projectile horizontalAttackProjectile,
                      Projectile verticalAttackProjectile,
                      Vector2 position = default,
                      int health = 75,
                      float moveSpeed = 1.25f) : base(image,
                                                      position,
                                                      health,
                                                      moveSpeed)
        {
            _horizontalAttackProjectile = horizontalAttackProjectile;
            _verticalAttackProjectile = verticalAttackProjectile;
            _attackDirection = Direction.Right;
            _attackTimer = 0;

            _idleFramesTimer = 0;
            _idleSecondsFrom = 1;
            _idleSecondsTo = 3;

            _AIstate = AIState.Idle;

            _targetPosition = position;
        }

        public override Shooter Clone() => new Shooter(Image,
                                                       _horizontalAttackProjectile,
                                                       _verticalAttackProjectile,
                                                       Position,
                                                       Health,
                                                       MoveSpeed);

        public override Projectile? GetAttack()
        {
            if (_attackTimer == 0)
            {
                if (_attackDirection == Direction.Right || _attackDirection == Direction.Left)
                {
                    _horizontalAttackProjectile.Position = Position + _attackDirection;
                    return _horizontalAttackProjectile.Clone();
                }
                else
                {
                    _verticalAttackProjectile.Position = Position + _attackDirection;
                    return _verticalAttackProjectile.Clone();
                }
            }
            else
                return null;
        }

        private void SetNewIdleFrames(int framesPerSecond)
        {
            _idleFramesTimer = Random.Shared.Next(_idleSecondsFrom * framesPerSecond, _idleSecondsTo * framesPerSecond);
        }

        public void UpdateAI(World world, Player player, int framesPerSecond)
        {
            switch (_AIstate)
            {
                case AIState.Idle:
                    if (CanSeeEntity(world, player))
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
                        Vector2.SqDistance(Position, _targetPosition) <= 9)
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

        public bool CanSeeEntity(World world, Entity entity)
        {
            Vector2 from, to;
            Direction deltaStep;
            if (entity.Position.X == Position.X)
            {
                (from, to) =
                    entity.Position.Y <= Position.Y
                    ? (entity.Position, Position)
                    : (Position, entity.Position);
                deltaStep = Direction.Down;
            }
            else if (entity.Position.Y == Position.Y)
            {
                (from, to) =
                    entity.Position.X <= Position.X
                    ? (entity.Position, Position)
                    : (Position, entity.Position);
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
