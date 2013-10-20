using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity
    {
        protected readonly Sprite Sprite;

        public Vector2 MapPosition { get; set; }

        public string Name { get; private set; }

        public short ZIndex { get; protected set; }

        protected Entity(string name, Sprite sprite, Vector2 mapPosition)
        {
            Name = name;
            Sprite = sprite;
            MapPosition = mapPosition;

            ZIndex = 0;
        }

        public virtual void Draw(Color light)
        {
            SpriteDrawer.Draw(Sprite, MapPosition, light);
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
    }
}
