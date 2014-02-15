using System.Collections.Generic;
using Engine.Drawing;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class DrawableEntity : Entity, IDrawingProvider
    {
        protected Sprite Sprite;

        protected DrawableEntity(string name, Sprite sprite, Coordinate coordinate)
            : base(name, coordinate)
        {
            Sprite = sprite;
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public void SetAnimation(string animationName, GameTime gameTime)
        {
            var animatedSprite = Sprite as AnimatedSprite;

            if (animatedSprite == null)
            {
                return;
            }

            animatedSprite.SetAnimation(animationName, gameTime);
        }

        public virtual IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, Coordinate.ToVector2(), Color.White);
        }
    }
}
