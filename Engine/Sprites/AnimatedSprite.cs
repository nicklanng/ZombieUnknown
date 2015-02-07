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

        public AnimatedSprite(string name, SpriteSheet spriteSheet, Vector2 center, AnimationList animationList)
            : base(name, spriteSheet, center)
        {
            _animationList = animationList;
            _currentAnimation = _animationList.Default();
            SpriteSheetRectangle = _currentAnimation[_frameIndex].Frame;
        }

        public override void Update()
        {
            if (_currentAnimation.AnimationType == AnimationType.RunOnce && _frameIndex == _currentAnimation.NumberOfFrames - 1)
            {
                return;
            }

            var now = GameState.GameTime.TotalGameTime;
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
            _frameIndex = _frameIndex % _currentAnimation.NumberOfFrames;
            SpriteSheetRectangle = _currentAnimation[_frameIndex].Frame;
            _timeEnteredAnimationFrame = gameTime.TotalGameTime;
        }
    }
}
