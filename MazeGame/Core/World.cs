using MazeGame.Core.GameObjects;
using MazeGame.Utils;

namespace MazeGame.Core
{
    class World
    {
        private Vector2 _worldSize;

        private Tile[,] _tileMap;
        private List<Entity> _entities;
        private List<Projectile> _projectiles;

        public World(Vector2 worldSize)
        {
            if (worldSize > Vector2.Zero)
                _worldSize = worldSize;
            else
                throw new ArgumentException();

            _tileMap = new Tile[worldSize.X, worldSize.Y];
            _entities = new List<Entity>();
            _projectiles = new List<Projectile>();
        }

        public Vector2 Size { get => _worldSize; }

        public IEnumerable<Entity> Entities => _entities;

        public IEnumerable<Projectile> Projectiles => _projectiles;

        public Tile this[int x, int y]
        {
            get => _tileMap[x, y];
            set => _tileMap[x, y] = value;
        }

        public Tile this[Vector2 position]
        {
            get => _tileMap[position.X, position.Y];
            set => _tileMap[position.X, position.Y] = value;
        }

        public void AddEntity(Entity entity)
        {
            if (InBounds(entity.Position))
                _entities.Add(entity);
            else
                throw new ArgumentException();
        }

        public void AddProjectile(Projectile projectile)
        {
            if (InBounds(projectile.Position))
                _projectiles.Add(projectile);
            else
                throw new ArgumentException();
        }

        public void ClearDeadEntities()
        {
            _entities.RemoveAll((entity) => entity.Health <= 0);
        }

        public void ClearAllGameObjects()
        {
            _entities.Clear();
            _projectiles.Clear();
        }

        public void UpdateProjectiles(int framesPerSecond)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Update(framesPerSecond);
                if (!this[projectile.Position].IsPassable)
                    projectile.Destroy();
            }
        }

        public void ClearDeadProjectile()
        {
            _projectiles.RemoveAll((projectile) => projectile.IsDead);
        }

        public List<Direction> GetNeighborsByCondition(Vector2 position, Func<Tile, bool> condition)
        {
            List<Direction> result = new List<Direction>(4);

            if (condition(this[position + Vector2.Up]))
                result.Add(Direction.Up);
            if (condition(this[position + Vector2.Right]))
                result.Add(Direction.Right);
            if (condition(this[position + Vector2.Down]))
                result.Add(Direction.Down);
            if (condition(this[position + Vector2.Left]))
                result.Add(Direction.Left);

            return result;
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
    }
}
