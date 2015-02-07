using System.Collections.Generic;
using Engine.Drawing;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class PhysicalEntity : Entity, IDrawingProvider
    {
        protected string CurrentAnimationType;

        protected Sprite Sprite;
        protected bool IsStatic = true;

        protected PhysicalEntity(string name, Sprite sprite, Vector2 mapPosition)
            : base(name, mapPosition)
        {
            Sprite = sprite.ShallowCopy();
        }

        public override void Update(GameTime gameTime)
        {
            var parentTile = GameState.Map.GetTile(MapPosition);
            if (parentTile != null) LightValue = parentTile.Light;
            Sprite.Update(gameTime);
        }

        public virtual void SetAnimation(string animationName)
        {
            if (animationName == CurrentAnimationType)
            {
                return;
            }

            CurrentAnimationType = animationName;

            UpdateAnimation();
        }

        public virtual IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, LightValue);
        }

        protected virtual void UpdateAnimation()
        {
            var animatedSprite = Sprite as AnimatedSprite;

            if (animatedSprite == null)
            {
                return;
            }

            animatedSprite.SetAnimation(CurrentAnimationType, GameState.GameTime);
        }
    }
}
