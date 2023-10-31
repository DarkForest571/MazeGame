using MazeGame.Graphics;

namespace MazeGame.Core
{
    sealed class Game
    {
        private World _world;
        private Generator _generator;

        private WorldRenderer _worldRenderer;

        public Game(Vector2 worldSize, Generator generator)
        {
            _world = new World(worldSize);
            _generator = generator;
            _worldRenderer = new CharWorldRenderer(worldSize);
        }


        public void CreateWorld()
        {
            _generator.Generate(_world);
        }

        public void RenderScene()
        {
            _worldRenderer.Render(_world);
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
