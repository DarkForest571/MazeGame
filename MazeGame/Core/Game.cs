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
            _worldRenderer = new WorldRenderer(_world, worldSize);
            _UIRenderer = new UIRenderer(new (5, 5), new Vector2(worldSize.X, 0), ' ');
            _UIRenderer.SetBorder('|','-','+');
        }

        public void CreateWorld()
        {
            _generator.Generate(_world);
        }

        public void RenderScene()
        {
            _worldRenderer.ClearBuffer();
            _worldRenderer.WorldToBuffer();
            _worldRenderer.Render();

            _UIRenderer.ClearBuffer();
            _UIRenderer.SetUIData("123456789123456789123456789");
            _UIRenderer.DataToBuffer(true);
            _UIRenderer.Render();
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
