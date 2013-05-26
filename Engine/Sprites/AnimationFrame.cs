using Microsoft.Xna.Framework;

namespace Engine.Sprites
{
    public class AnimationFrame
    {
        public Rectangle Frame { get; private set; }

        public double Duration { get; private set; }

        public AnimationFrame(Rectangle frame, double duration)
        {
            Frame = frame;
            Duration = duration;
        }
    }
}
