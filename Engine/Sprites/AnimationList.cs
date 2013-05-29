using System.Collections.Generic;

namespace Engine.Sprites
{
    public class AnimationList
    {
        private readonly Dictionary<string, Animation> _animations;
        private Animation _defaultAnimation;

        public AnimationList()
        {
            _animations = new Dictionary<string, Animation>();
        }

        public Animation Default()
        {
            return _defaultAnimation;
        }

        public void Add(string name, Animation animation)
        {
            _animations.Add(name, animation);

            if (_defaultAnimation == null)
            {
                _defaultAnimation = animation;
            }
        }

        public Animation GetAnimation(string name)
        {
            return _animations[name];
        }
    }
}
