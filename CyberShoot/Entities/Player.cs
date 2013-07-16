using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CyberShoot.Entities
{
    public class Player
    {
        protected readonly Sprite Sprite;

        public string Name { get { return "Player"; } }

        protected Player(Sprite sprite)
        {
            Sprite = sprite;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Sprite.Draw(spriteBatch, position);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
    }
}
