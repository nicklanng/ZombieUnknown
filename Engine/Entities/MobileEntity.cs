using System.Collections.Generic;
using System.Linq;
using Engine.AI.Steering;
using Engine.Extensions;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class MobileEntity : VisibleEntity, IActor
    {
        public Queue<Vector2> PreviousVelocities { get; protected set; } 

        public abstract float MaxVelocity { get; }
        public Vector2 Velocity { get; private set; }
        public float Radius { get { { return 0.5f; }}}

        public SeekBehavior SeekBehavior { get; set; }
        public AvoidActorsBehavior AvoidActorsBehavior { get; set; }
        public FollowPathBehavior FollowPathBehavior { get; set; }
        public AvoidanceBehavior AvoidanceBehavior { get; set; }
        public ContainmentBehavior ContainmentBehavior { get; set; }
        public QueueBehavior QueueBehavior { get; set; }

        public IDirection FacingDirection { get; protected set; }

        protected MobileEntity(string name, Sprite sprite, Vector2 mapPosition) : base(name, sprite, mapPosition)
        {
            PreviousVelocities = new Queue<Vector2>();

            IsStatic = false;
            FacingDirection = Direction.North;
            CurrentAnimationType = "idle";
            Velocity = Vector2.Zero;
        }
        
        public override void Update()
        {
            const int previousVelocityCount = 50;

            var seekVector = SeekBehavior == null ? Vector2.Zero : SeekBehavior.GetForce(this);
            var avoidActorsVector = AvoidActorsBehavior == null ? Vector2.Zero : AvoidActorsBehavior.GetForce(this);
            var followPathVector = FollowPathBehavior == null ? Vector2.Zero : FollowPathBehavior.GetForce(this);
            
            var currentVector = (seekVector + avoidActorsVector + followPathVector).Truncate(1);
            var avoidanceVector = AvoidanceBehavior == null ? Vector2.Zero : AvoidanceBehavior.GetForce(this, currentVector);

            currentVector = (seekVector + avoidActorsVector + followPathVector + avoidanceVector).Truncate(1);
            var containmentVector = ContainmentBehavior == null ? Vector2.Zero : ContainmentBehavior.GetForce(this, currentVector);

            currentVector = (seekVector + avoidActorsVector + followPathVector + avoidanceVector).Truncate(1);
            var queueVector = QueueBehavior == null ? Vector2.Zero : QueueBehavior.GetForce(this, currentVector);

            currentVector = (seekVector + avoidActorsVector + followPathVector + avoidanceVector + containmentVector + queueVector).Truncate(1);
            Velocity = currentVector * MaxVelocity;


            MapPosition = MapPosition + Velocity;

            var intendedMovement = (seekVector + avoidActorsVector + followPathVector + avoidanceVector + containmentVector);
            PreviousVelocities.Enqueue(intendedMovement);
            while (PreviousVelocities.Count > previousVelocityCount) PreviousVelocities.Dequeue();

            intendedMovement = Vector2.Zero;
            for (var i = 0; i < PreviousVelocities.Count; i++)
            {
                intendedMovement += PreviousVelocities.ElementAt(i);
            }
            intendedMovement /= previousVelocityCount;
            if (intendedMovement != Vector2.Zero)
            {
                var direction = Direction.GetDirectionFromVector(intendedMovement * 1000);
                FaceDirection(direction);
            }

            base.Update();
        }

        public virtual void ForceFaceDirection(IDirection direction)
        {
            FacingDirection = direction;

            UpdateAnimation();
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

