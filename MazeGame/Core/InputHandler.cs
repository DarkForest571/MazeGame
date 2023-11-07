using MazeGame.Utils;

namespace MazeGame.Core
{
    interface IInputHandler
    {
        public void PullInput();

        public IEnumerable<PlayerCommand> Commands { get; }

        public void ClearCommands();

        public bool ExitCommand { get; }
    }

    class DefaultInputHandler : IInputHandler
    {
        private LinkedList<PlayerCommand> _playerCommands;
        private bool _exitCommand;

        public DefaultInputHandler()
        {
            _playerCommands = new LinkedList<PlayerCommand>();
            _exitCommand = false;
        }

        public bool ExitCommand => _exitCommand;

        public IEnumerable<PlayerCommand> Commands => _playerCommands;

        public void PullInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.W:
                        _playerCommands.AddLast(PlayerCommand.GoUp);
                        break;
                    case ConsoleKey.A:
                        _playerCommands.AddLast(PlayerCommand.GoLeft);
                        break;
                    case ConsoleKey.S:
                        _playerCommands.AddLast(PlayerCommand.GoDown);
                        break;
                    case ConsoleKey.D:
                        _playerCommands.AddLast(PlayerCommand.GoRight);
                        break;
                    case ConsoleKey.Spacebar:
                        _playerCommands.AddLast(PlayerCommand.Attack);
                        break;
                    case ConsoleKey.Escape:
                        _exitCommand = true;
                        break;
                }
            }
        }

        public void ClearCommands()
        {
            _playerCommands.Clear();
        }
    }
}
