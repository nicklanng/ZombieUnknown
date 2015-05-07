using System.Collections.Generic;
using Engine.Drawing;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class VisibleEntity : PhysicalEntity, IDrawingProvider
    {
        protected Sprite Sprite;
        protected Color LightValue { get; set; }

        protected VisibleEntity(string name, Sprite sprite, Vector2 mapPosition) 
            : base(name, mapPosition)
        {
            Sprite = sprite.ShallowCopy();
        }

        public override void Update()
        {
            base.Update();

            var parentTile = GameState.Map.GetTile(MapPosition);
            if (parentTile != null) LightValue = parentTile.Light;

            //if (GameState.PathfindingMap.IsPointOutOfBounds(MapPosition)) LightValue = Color.Red;
            Sprite.Update();
        }

        public virtual IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, LightValue);
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

        protected virtual void UpdateAnimation()
        {
            var animatedSprite = Sprite as AnimatedSprite;

            if (animatedSprite == null)
            {
                return;
            }

            animatedSprite.SetAnimation(CurrentAnimationType, GameState.GameTime);
        }

        public bool IsAnimationComplete()
        {
            var animatedSprite = Sprite as AnimatedSprite;

            if (animatedSprite == null)
            {
                return true;
            }

            return animatedSprite.IsAnimationComplete();
        }
    }
}
