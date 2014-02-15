using System;
using Microsoft.Xna.Framework;

namespace Engine.Sprites
{
    public class AnimatedSprite : Sprite
    {
        private readonly AnimationList _animationList;
        private Animation _currentAnimation;
        private TimeSpan _timeEnteredAnimationFrame;
        private int _frameIndex;

        public AnimatedSprite(string name, SpriteSheet spriteSheet, Vector2 center, BoundingBox localBoundingBox, AnimationList animationList)
            : base(name, spriteSheet, center, localBoundingBox)
        {
            _animationList = animationList;
            _currentAnimation = _animationList.Default();
            SpriteSheetRectangle = _currentAnimation[_frameIndex].Frame;
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentAnimation.AnimationType == AnimationType.RunOnce && _frameIndex == _currentAnimation.NumberOfFrames - 1)
            {
                return;
            }

            var now = gameTime.TotalGameTime;
            var timeSinceLastFrame = now - _timeEnteredAnimationFrame;
            if (timeSinceLastFrame.TotalSeconds >= _currentAnimation[_frameIndex].Duration)
            {
                _timeEnteredAnimationFrame = _timeEnteredAnimationFrame.Add(TimeSpan.FromSeconds(_currentAnimation[_frameIndex].Duration));

                _frameIndex++;
                _frameIndex = _frameIndex % _currentAnimation.NumberOfFrames;
                SpriteSheetRectangle = _currentAnimation[_frameIndex].Frame;
            }
        }

        public void SetAnimation(string animationName, GameTime gameTime)
        {
            _currentAnimation = _animationList.GetAnimation(animationName);
            SpriteSheetRectangle = _currentAnimation[_frameIndex].Frame;
            _frameIndex = 0;
            _timeEnteredAnimationFrame = gameTime.TotalGameTime;
        }
    }
}
