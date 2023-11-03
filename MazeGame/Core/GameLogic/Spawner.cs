using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface Spawner
    {
        public void SpawnOne();

        public void SpawnN(int count);

        public void SpawnAll();

        public Entity Entity { get; set; }

        public int Count { get; set; }
    }

    sealed class WorldwiseSpawner : Spawner
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

        public void SpawnOne()
        {
            _entity.Position = _world.GetRandomTileByCondition((tile) => tile is PassableTile);
            _world.AddEntity(_entity.Clone());
        }

        public void SpawnN(int count)
        {
            for (int i = 0; i < count; i++)
                SpawnOne();
        }

        public void SpawnAll() => SpawnN(_count);
    }
}
