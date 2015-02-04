using System.Collections.Generic;
using Engine.Drawing;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class PhysicalEntity : Entity, IDrawingProvider
    {
        private string _currentAnimationType;

        protected Sprite Sprite;
        protected bool IsStatic = true;

        public abstract float Speed { get; }

        public bool IsRunning { set; get; }
        public IDirection FacingDirection { get; protected set; }

        protected PhysicalEntity(string name, Sprite sprite, Vector2 mapPosition)
            : base(name, mapPosition)
        {
            Sprite = sprite.ShallowCopy();
            FacingDirection = Direction.North;
            _currentAnimationType = "idle";
        }

        public override void Update(GameTime gameTime)
        {
            var parentTile = GameState.Map.GetTile(MapPosition);
            if (parentTile != null) LightValue = parentTile.Light;
            Sprite.Update(gameTime);
        }

        public void SetAnimation(string animationName)
        {
            if (animationName == _currentAnimationType)
            {
                return;
            }

            _currentAnimationType = animationName;

            UpdateAnimation();
        }

        public virtual IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, MapPosition, LightValue);
        }

        public virtual void FaceDirection(IDirection direction)
        {
            if (IsStatic) 
            {
                return;
            }

            if (direction == FacingDirection) 
            {
                return;
            }

            FacingDirection = direction;

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            var animatedSprite = Sprite as AnimatedSprite;

            if (animatedSprite == null)
            {
                return;
            }

            var animationId = _currentAnimationType + FacingDirection;

            animatedSprite.SetAnimation(animationId, GameState.GameTime);
        }
    }
}
