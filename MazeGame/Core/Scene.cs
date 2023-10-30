using MazeGame.Graphics;

namespace MazeGame.Core
{
    sealed class Scene
    {
        private World _world;
        private Generator _generator;

        public Scene(Vector2 worldSize, Generator generator)
        {
            _world = new World(worldSize);
            _generator = generator;
        }


        public void CreateWorld()
        {
            _generator.Generate(_world);
        }

        public void RenderScene()
        {

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
