using System.Linq;
using Engine.Entities;
using Engine.Entities.Interactions;

namespace Engine.AI.BehaviorTrees.Actions
{
    public abstract class InteractAction<TInteraction, TSubject, TActor> : BehaviorAction where TSubject : PhysicalEntity
                                                                                          where TActor : MobileEntity
                                                                                          where TInteraction : IInteraction<TSubject, TActor>
    {
        private IInteraction<TSubject, TActor> _interaction;

        public IInteraction<TSubject, TActor> Interaction
        {
            get { return _interaction ?? (_interaction = InteractionManager.GetInteractionOfType<TInteraction>()); }
        }

        protected sealed override GoalStatus Action(Blackboard blackboard)
        {
            var actor = (TActor)blackboard["subject"];
            var subject = (TSubject)GameState.InteractionObject;

            if (SavedResult == GoalStatus.Inactive)
            {
                var accessPositions = subject.AccessPositions;

                var accessPosition = accessPositions.SingleOrDefault(x => (x.PositionOffset + subject.MapPosition) == actor.MapPosition);
                if (accessPosition == null)
                {
                    return GoalStatus.Failed;
                }

                var requiredDirection = accessPosition.Direction;
                actor.TransitionState("interact");
                actor.FaceDirection(requiredDirection);

                blackboard["TimeWhenInteractionFinished"] = GameState.GameTime.TotalGameTime.TotalMilliseconds + Interaction.MillisToCompleteAction;

                return GoalStatus.Running;
            }

            if (SavedResult == GoalStatus.Running)
            {
                var timeWhenInteractionFinished = (double)blackboard["TimeWhenInteractionFinished"];
                var timeNow = GameState.GameTime.TotalGameTime.TotalMilliseconds;

                if (timeNow >= timeWhenInteractionFinished)
                {
                    actor.TransitionState("idle");
                    Interaction.Interact(subject, actor);

                    return GoalStatus.Completed;
                }
            }

            return GoalStatus.Running;
        }
    }
}