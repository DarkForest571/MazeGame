using MazeGame.Graphics;

namespace MazeGame.Core
{
    sealed class Game
    {
        private World _world;
        private Generator _generator;

        private WorldRenderer _worldRenderer;
        private UIRenderer _UIRenderer;

        public Game(Vector2 worldSize, Generator generator)
        {
            _world = new World(worldSize);
            _generator = generator;
            _worldRenderer = new ConsoleWorldRenderer(worldSize);
            _UIRenderer = new ConsoleUIRenderer(new Vector2(20, 10), new Vector2(worldSize.X + 5, 5));
        }

        public void CreateWorld()
        {
            _generator.Generate(_world);
        }

        public void RenderScene()
        {
            _worldRenderer.Render(_world);
            _UIRenderer.Render("1111111111111111111111111111");
        }

        // Entitiy manager

        //public bool PlaceEntity(Entity entity)
        //{
        //    if (TileAt(entity.Position) is PassableTile)
        //    {
        //        _entities.AddLast(entity);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
