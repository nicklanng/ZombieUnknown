using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.AI.Steering
{
    public interface IActor
    {
        float MaxVelocity { get; }
        Vector2 MapPosition { get; }
        Vector2 Velocity { get; }
        float Radius { get; }

        SeekBehavior SeekBehavior { get; set; }
        AvoidActorsBehavior AvoidActorsBehavior { get; set; }
        FollowPathBehavior FollowPathBehavior { get; set; }
        AvoidanceBehavior AvoidanceBehavior { get; set; }
        ContainmentBehavior ContainmentBehavior { get; set; }

        void FaceDirection(IDirection direction);
    }
}
