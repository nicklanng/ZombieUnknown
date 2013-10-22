using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity
    {
        protected readonly Sprite Sprite;

        public Coordinate Coordinate { get; set; }

        public string Name { get; private set; }

        public short ZIndex { get; protected set; }

        protected Entity(string name, Sprite sprite, Coordinate coordinate)
        {
            Name = name;
            Sprite = sprite;
            Coordinate = coordinate;

            ZIndex = 0;
        }

        public virtual void Draw(Color light)
        {
            SpriteDrawer.Draw(Sprite, Coordinate.ToVector2(), light);
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
    }
}
