using System.Collections.ObjectModel;

using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core
{
    class World
    {
        private Vector2 _worldSize;

        private Tile[,] _map;
        private List<Creature> _creatures;

        public World(Vector2 worldSize)
        {
            if (worldSize > Vector2.Zero)
                _worldSize = worldSize;
            else
                throw new ArgumentException();

            _map = new Tile[worldSize.X, worldSize.Y];
            _creatures = new List<Creature>();
        }

        public Vector2 Size { get => _worldSize; }

        public Tile this[int x, int y]
        {
            get => _map[x, y];
            set => _map[x, y] = value;
        }

        public Tile this[Vector2 position]
        {
            get => _map[position.X, position.Y];
            set => _map[position.X, position.Y] = value;
        }

        public void AddCreature(Creature entity)
        {
            if (InBounds(entity.Position))
                _creatures.Add(entity);
            else
                throw new ArgumentException();
        }

        public void ClearDeadCreatures()
        {
            _creatures.RemoveAll((entity) => entity.Health <= 0);
        }

        public void RemoveAllCreatures()
        {
            _creatures.Clear();
        }

        public Vector2 GetRandomPositionByCondition(Func<Tile, bool> condition)
        {
            Vector2 position = new Vector2();
            do
            {
                position.X = Random.Shared.Next(Size.X);
                position.Y = Random.Shared.Next(Size.Y);
            } while (!condition(this[position]));
            return position;
        }

        public bool InBounds(Vector2 position) => position >= Vector2.Zero && position < _worldSize;

        public ReadOnlyCollection<Creature> Creatures => _creatures.AsReadOnly();
    }
}
