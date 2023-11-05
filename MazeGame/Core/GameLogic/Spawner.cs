using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core.GameLogic
{
    interface ISpawner
    {
        public Creature SpawnOne();

        public void SpawnAll();

        public Creature Creature { get; set; }

        public int Count { get; set; }
    }

    sealed class WorldwiseSpawner : ISpawner
    {
        private World _world;
        private Creature _creature;
        private int _count;

        public WorldwiseSpawner(World world, Creature entity, int count = 5)
        {
            _world = world;
            _creature = entity;
            _count = count;
        }

        public Creature Creature { get => _creature; set => _creature = value; }

        public int Count { get => _count; set => _count = value; }

        public Creature SpawnOne()
        {
            _creature.Position = _world.GetRandomPositionByCondition((tile) => tile is PassableTile);
            Creature creature = _creature.Clone();
            _world.AddCreature(creature);
            return creature;
        }

        public void SpawnAll()
        {
            for (int i = 0; i < _count; i++)
                SpawnOne();
        }
    }
}
