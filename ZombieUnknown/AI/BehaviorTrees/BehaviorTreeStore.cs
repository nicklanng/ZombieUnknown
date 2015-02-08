using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.AI.BehaviorTrees.Composites;
using Engine.AI.BehaviorTrees.Decorators;
using Engine.AI.BehaviorTrees.SubTrees;
using ZombieUnknown.AI.BehaviorTrees.Actions;
using ZombieUnknown.AI.BehaviorTrees.Conditionals;
using ZombieUnknown.AI.BehaviorTrees.SubTrees;

namespace ZombieUnknown.AI.BehaviorTrees
{
    public static class BehaviorTreeStore
    {
        public static Behavior HumanBehavior { get; private set; }

        public static void Generate()
        {
            var wholeThingSequence = new Sequence(new CreateRandomMovementTargetAction(), new CalculateRouteAction(), new FollowPathSubTree());
          
            var walkToInteractionObject = new Inverter(new FollowPathSubTree());
            
            var interationSequence = new Sequence(new TryToDieSubTree(), new NeedToEatConditional(), new GetInteractionObjectAction(), new CalculateRouteAction(), walkToInteractionObject, new GetFoodInteractAction(), wholeThingSequence);
            var root = new Repeater(interationSequence);


            HumanBehavior = new Behavior(root);
        }

    }
}
