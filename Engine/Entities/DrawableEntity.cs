using System.Collections.Generic;
using Engine.Drawing;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class DrawableEntity : Entity, IDrawingProvider
    {
        private string _currentAnimationType;

        protected Sprite Sprite;
        protected bool IsStatic = true;

        public abstract float Speed { get; }

        public IDirection FacingDirection { get; protected set; }

        protected DrawableEntity(string name, Sprite sprite, Coordinate coordinate)
            : base(name, coordinate)
        {
            Sprite = sprite;
            FacingDirection = Direction.North;
            _currentAnimationType = "idle";
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public void SetAnimation(string animationName, GameTime gameTime)
        {
            if (animationName == _currentAnimationType)
            {
                return;
            }

            _currentAnimationType = animationName;

            UpdateAnimation(gameTime);
        }

        public virtual IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, GetCoordinate(), Color.White);
        }

        public virtual void FaceDirection(IDirection direction, GameTime gameTime)
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

            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            var animatedSprite = Sprite as AnimatedSprite;

            if (animatedSprite == null)
            {
                return;
            }

            var animationId = _currentAnimationType + FacingDirection.ToString();

            animatedSprite.SetAnimation(animationId, gameTime);
        }
    }
}
