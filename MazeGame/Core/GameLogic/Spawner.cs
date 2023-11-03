using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface ISpawner
    {
        public Entity SpawnOne();

        public void SpawnAll();

        public Entity Entity { get; set; }

        public int Count { get; set; }
    }

    sealed class WorldwiseSpawner : ISpawner
    {
        private World _world;
        private Entity _entity;
        private int _count;

        public WorldwiseSpawner(World world, Entity entity, int count = 5)
        {
            _world = world;
            _entity = entity;
            _count = count;
        }

        public Entity Entity { get => _entity; set => _entity = value; }

        public int Count { get => _count; set => _count = value; }

        public Entity SpawnOne()
        {
            _entity.Position = _world.GetRandomTileByCondition((tile) => tile is PassableTile);
            Entity entity = _entity.Clone();
            _world.AddEntity(entity);
            return entity;
        }

        public void SpawnAll()
        {
            for (int i = 0; i < _count; i++)
                SpawnOne();
        }
    }
}
