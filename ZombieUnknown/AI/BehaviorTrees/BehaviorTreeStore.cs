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
        public static Behavior ZombieBehavior { get; private set; }

        public static void Generate()
        {
            BuildHumanBehaviorTree();
            BuildZombieBehaviorTree();
        }

        private static void BuildHumanBehaviorTree()
        {
            var eatFood = new Sequence(new GetFoodSourceAction(), new CalculateRouteAction(),
                new Inverter(new FollowPathSubTree()), new GetFoodInteractAction());
            var wander = new Sequence(new CreateRandomMovementTargetAction(), new CalculateRouteAction(),
                new Inverter(new FollowPathSubTree()));

            var interationSequence = new Sequence(new TryToDieSubTree(), new NeedToEatConditional(), eatFood, wander);
            var root = new Repeater(interationSequence);
            HumanBehavior = new Behavior(root);
        }

        private static void BuildZombieBehaviorTree()
        {
            var interationSequence = new Sequence(new GetHumanAction(), new ZombieCalculateRouteAction(), new FollowPathSubTree());
            var root = new Repeater(interationSequence);
            ZombieBehavior = new Behavior(root);
        }
    }
}
