using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity
    {
        protected readonly ISpriteDrawer _spriteDrawer;

        protected readonly Sprite Sprite;

        public Vector2 MapPosition { get; set; }

        public string Name { get; private set; }

        public short ZIndex { get; protected set; }

        protected Entity(string name, Sprite sprite, ISpriteDrawer spriteDrawer)
        {
            Name = name;
            Sprite = sprite;
            _spriteDrawer = spriteDrawer;

            ZIndex = 0;
        }

        public virtual void Draw(Color light)
        {
            _spriteDrawer.Draw(Sprite, MapPosition, light);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
    }
}
