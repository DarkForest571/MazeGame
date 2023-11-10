using MazeGame.Core.GameObjects;

namespace MazeGame.Core
{
    interface ISpawner
    {
        public Entity SpawnOne();

        public void SpawnAll();

        public Entity Entitiy { get; set; }

        public int Count { get; set; }
    }

    sealed class WorldwiseSpawner : ISpawner
    {
        private World _world;
        private Entity _entities;
        private int _count;

        public WorldwiseSpawner(World world, Entity entity, int count)
        {
            _world = world;
            _entities = entity;
            _count = count;
        }

        public Entity Entitiy { get => _entities; set => _entities = value; }

        public int Count { get => _count; set => _count = value; }

        public Entity SpawnOne()
        {
            _entities.Position = _world.GetRandomPositionByCondition((tile) => tile.IsPassable);
            Entity entity = _entities.Clone();
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
