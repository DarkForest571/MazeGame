using MazeGame.Utils;

namespace MazeGame.Core.GameObjects
{
    sealed class Player : Entity
    {
        private readonly Projectile _attackProjectile;
        private Direction _attackDirection;
        private int _attackTimer;
        private float _secondsPerAttack;

        public Player(char playerImage,
                      Projectile attackProjectile,
                      int health = 100,
                      float moveSpeed = 1.0f,
                      float secondsPerAttack = 1.0f,
                      Vector2 position = default) : base(playerImage,
                                                         health,
                                                         moveSpeed,
                                                         position)
        {
            _attackProjectile = attackProjectile;
            _attackDirection = Direction.Right;
            _attackTimer = 0;
            _secondsPerAttack = secondsPerAttack;
        }

        public override Player Clone() => new Player(Image, _attackProjectile, Health, MoveSpeed, _secondsPerAttack, Position);

        public Projectile GetAttack()
        {
            return _attackProjectile.Clone();
        }

        public void Update(World world, IInputHandler inputHandler, int framesPerSecond)
        {
            UpdateAttackTimer();
            UpdateMoveTimer();

            foreach (PlayerCommand command in inputHandler.Commands)
            {
                switch (command)
                {
                    case PlayerCommand.GoUp:
                    case PlayerCommand.GoRight:
                    case PlayerCommand.GoDown:
                    case PlayerCommand.GoLeft:
                        Direction directionToMove = command switch
                        {
                            PlayerCommand.GoUp => Direction.Up,
                            PlayerCommand.GoRight => Direction.Right,
                            PlayerCommand.GoDown => Direction.Down,
                            PlayerCommand.GoLeft => Direction.Left
                        };
                        _attackDirection = directionToMove;
                        List<Direction> availableDirections =
                            world.GetNeighborsByCondition(Position, (tile) => tile.IsPassable);
                        if (availableDirections.Contains(directionToMove))
                        {
                            MoveTo(directionToMove, world[Position + directionToMove].MoveCost, framesPerSecond);
                        }
                        break;
                    case PlayerCommand.Attack:
                        if (_attackTimer == 0)
                        {
                            _attackTimer = (int)(_secondsPerAttack * framesPerSecond);
                            Projectile projectile = GetAttack();
                            projectile.Position = Position + _attackDirection;
                            projectile.Init(this, _attackDirection, framesPerSecond);
                            world.AddProjectile(projectile);
                        }
                        break;
                }
            }
            inputHandler.ClearCommands();
        }

        private void UpdateAttackTimer()
        {
            if (_attackTimer > 0)
                _attackTimer--;
        }
    }
}
