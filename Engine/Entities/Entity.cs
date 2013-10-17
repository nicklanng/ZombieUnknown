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

        protected Entity(string name, Sprite sprite)
        {
            Name = name;
            Sprite = sprite;

            ZIndex = 0;
        }

        public virtual void Draw(Color light)
        {
            SpriteDrawer.Draw(Sprite, MapPosition, light);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
    }
}
