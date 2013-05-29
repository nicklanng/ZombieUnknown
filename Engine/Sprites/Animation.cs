using System.Collections.Generic;

namespace Engine.Sprites
{
    public class Animation
    {
        public AnimationType AnimationType { get; private set; }
        private readonly List<AnimationFrame> _frames;
        
        public int NumberOfFrames
        {
            get { return _frames.Count; }
        }

        public Animation(AnimationType animationType)
        {
            AnimationType = animationType;
            _frames = new List<AnimationFrame>();
        }

        public void AddFrame(AnimationFrame frame)
        {
            _frames.Add(frame);
        }

        public AnimationFrame this[int i]
        {
            get { return _frames[i]; }
        }
    }
}
