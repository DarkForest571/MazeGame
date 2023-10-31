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
            _UIRenderer = new UIRenderer(new(20, 10), new Vector2(worldSize.X, 0), ' ');
        }

        public void Init()
        {
            _UIRenderer.SetBorder('|', '=', 'o');
            _generator.Generate(_world);
        }

        public void CreateWorld()
        {
            _generator.Generate(_world);
        }

        public void RenderScene()
        {
            Console.CursorVisible = false;
            _worldRenderer.ClearBuffer();
            _worldRenderer.WorldToBuffer();
            _worldRenderer.Render();

            _UIRenderer.ClearBuffer();
            _UIRenderer.SetUIData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras sit amet consectetur velit. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam ligula orci, scelerisque quis risus non, congue faucibus neque.");
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
