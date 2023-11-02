namespace MazeGame.Core
{
    interface GameController
    {
        public void UpdateAI();

        public void UpdateEntities();
    }

    sealed class MazeGameController : GameController
    {
        private World _world;

        public MazeGameController(World world)
        {
            _world = world;
        }

        public void UpdateAI()
        {

        }

        public void UpdateEntities()
        {

        }
    }
}
