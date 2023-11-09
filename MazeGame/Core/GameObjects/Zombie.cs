using MazeGame.Core.GameLogic;
using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Zombie : Entity, IAIControlable
    {
        private readonly Projectile _attackProjectile;
        private Direction _attackDirection;
        private int _attackTimer;

        private int _idleFramesTimer;
        private int _idleSecondsFrom;
        private int _idleSecondsTo;

        private AIState _AIstate;

        private Vector2 _targetPosition;

        public Zombie(char zombieImage,
                      Projectile attackProjectile,
                      Vector2 position = default,
                      int health = 150,
                      float moveSpeed = 0.25f) : base(zombieImage,
                                                      position,
                                                      health,
                                                      moveSpeed)
        {
            _attackProjectile = attackProjectile;
            _attackDirection = Direction.Right;
            _attackTimer = 0;

            _idleFramesTimer = 0;
            _idleSecondsFrom = 1;
            _idleSecondsTo = 3;

            _AIstate = AIState.Idle;

            _targetPosition = position;
        }

        public int IdleTimer => _idleFramesTimer;

        public AIState AIState { get => _AIstate; set => _AIstate = value; }

        public Vector2 TargetPosition { get => _targetPosition; set => _targetPosition = value; }

        public override Zombie Clone() => new Zombie(Image, _attackProjectile, Position, Health, MoveSpeed);

        public void TickIdleTimer()
        {
            if (_idleFramesTimer > 0)
                _idleFramesTimer--;
        }

        public void SetNewIdleFrames(int framesPerSecond)
        {
            _idleFramesTimer = Random.Shared.Next(_idleSecondsFrom * framesPerSecond, _idleSecondsTo * framesPerSecond);
        }

        public override Projectile? GetAttack()
        {
            if (_attackTimer == 0)
            {
                _attackProjectile.Position = Position + _attackDirection;
                return _attackProjectile.Clone();
            }
            else
                return null;
        }

        public void UpdateAI(World world, Player player, int framesPerSecond)
        {
            switch (_AIstate)
            {
                case AIState.Idle:
                    if (false)
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
    }
}
