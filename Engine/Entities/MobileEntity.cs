using System;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class MobileEntity : PhysicalEntity
    {
        public abstract float Speed { get; }

        public bool IsRunning { set; get; }
        public IDirection FacingDirection { get; protected set; }

        public MobileEntity(string name, Sprite sprite, Vector2 mapPosition) : base(name, sprite, mapPosition)
        {
            IsStatic = false;
            FacingDirection = Direction.North;
            CurrentAnimationType = "idle";
        }

        public virtual void FaceDirection(IDirection direction)
        {
            if (direction == FacingDirection) 
            {
                return;
            }

            FacingDirection = direction;

            UpdateAnimation();
        }

        protected override void UpdateAnimation()
        {
            var animatedSprite = Sprite as AnimatedSprite;
            if (animatedSprite == null)
            {
                return;
            }

            var animationId = CurrentAnimationType + FacingDirection;

            animatedSprite.SetAnimation(animationId, GameState.GameTime);
        }
    }
}

