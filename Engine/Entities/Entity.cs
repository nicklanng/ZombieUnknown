using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity
    {
        public Vector2 MapPosition { get; set; }
        public Coordinate Coordinate { get; set; }
        public string Name { get; private set; }
        public short ZIndex { get; protected set; }

        protected Entity(string name,Coordinate coordinate)
        {
            Name = name;
            Coordinate = coordinate;
            MapPosition = Coordinate.ToVector2();

            ZIndex = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
