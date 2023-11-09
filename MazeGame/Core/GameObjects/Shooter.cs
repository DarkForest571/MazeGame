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

        private Vector2 _playerPosition;
        private Vector2 _attackPosition;

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

            _playerPosition = position;
        }

        public int IdleTimer => _idleFramesTimer;

        public AIState AIState { get => _AIstate; set => _AIstate = value; }

        public Vector2 TargetPosition { get => _playerPosition; set => _playerPosition = value; }

        public override Shooter Clone() => new Shooter(Image,
                                                       _horizontalAttackProjectile,
                                                       _verticalAttackProjectile,
                                                       Position,
                                                       Health,
                                                       MoveSpeed);
        
        public void TickIdleTimer()
        {
            if (_idleFramesTimer > 0)
                _idleFramesTimer--;
        }

        public void SetNewIdleFrames(int framesPerSecond)
        {
            _idleFramesTimer = Random.Shared.Next(_idleSecondsFrom * framesPerSecond, _idleSecondsTo * framesPerSecond);
        }

        public void SetWanderingPosition(World world)
        {
            List<Direction> list = world.GetNeighborsByCondition(Position, (tile) => tile.IsPassable);
            if (list.Count > 0)
            {
                int choise = Random.Shared.Next(list.Count);
                _playerPosition = Position + list[choise];
            }
            else
            {
                _playerPosition = Position;
            }
        }

        public bool IsOnAttackPosition() => Position == _attackPosition;

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

        public void AIAction(World world, Player player, int framesPerSecond)
        {
            switch (_AIstate)
            {
                case AIState.Idle:
                case AIState.Follow:
                    if (MoveTimer == 0)
                    {
                        Direction moveDirection = Vector2.GetDirection(Position, _playerPosition);
                        MoveTo(moveDirection, world[Position + moveDirection].MoveCost);
                    }
                    break;
                case AIState.Attack:
                    break;
            }
        }
    }
}
