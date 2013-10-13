using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Isometric.Entities
{
    public abstract class Entity
    {
        protected readonly Sprite Sprite;

        public string Name { get; private set; }
        public Tile Parent { get; set; } // see if I can remove this

        public short ZIndex { get; protected set; }

        protected Entity(string name, Sprite sprite)
        {
            Name = name;
            Sprite = sprite;

            ZIndex = 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color light)
        {
            Sprite.Draw(spriteBatch, position, light);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
    }
}
