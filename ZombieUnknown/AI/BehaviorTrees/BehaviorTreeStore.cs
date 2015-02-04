using Engine.AI.BehaviorTrees;
using Engine.AI.BehaviorTrees.Actions;
using Engine.AI.BehaviorTrees.Composites;
using Engine.AI.BehaviorTrees.Conditionals;
using Engine.AI.BehaviorTrees.Decorators;
using ZombieUnknown.AI.BehaviorTrees.Actions;

namespace ZombieUnknown.AI.BehaviorTrees
{
    public static class BehaviorTreeStore
    {
        public static Behavior HumanBehavior { get; set; }

        public static void Generate()
        {
            var hasPathConditional = new HasPathConditional();
            var followPathAction = new FollowPathAction();

            var createRandomRoute = new CreateRandomMovementTargetAction();
            var calculateRouteAction = new CalculateRouteAction();

            var idleAction = new IdleAction();
            var followPathSequence = new Sequence(hasPathConditional, followPathAction, idleAction);
            var followPathRepeater = new RepeaterUntilFail(followPathSequence);

            var wholeThingSequence = new Sequence(createRandomRoute, calculateRouteAction, followPathRepeater);

            var wholeThingRepeater = new Repeater(wholeThingSequence);

            HumanBehavior = new Behavior(wholeThingRepeater);
        }

    }
}
