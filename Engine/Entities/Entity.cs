using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity
    {
        public Vector2 MapPosition { get; set; }
        private Coordinate _coordinate;

        public virtual void SetCoordinate(Coordinate value)
        {
            _coordinate = value;
        }

        public Coordinate GetCoordinate()
        {
            return _coordinate;
        }

        public string Name { get; private set; }
        public short ZIndex { get; protected set; }

        protected Entity(string name, Coordinate coordinate)
        {
            Name = name;
            _coordinate = coordinate;
            MapPosition = GetCoordinate();

            ZIndex = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
