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

            var harvestWheat = new Sequence(new GetGrownWheatTargetAction(), new CanHarvestWheatConditional(), new CalculateRouteAction(),
                                            new Inverter(new FollowPathSubTree()), new HarvestWheatInteractAction());

            var wander = new Sequence(new CreateRandomMovementTargetAction(), new CalculateRouteAction(),
                                      new Inverter(new FollowPathSubTree()));

            var plantWheat = new Sequence(new CanPlantWheatConditional(),
                                          new GetCultivatedLandTargetAction(),
                                          new CalculateRouteAction(),
                                          new Inverter(new FollowPathSubTree()),
                                          new SowSeedInteractAction(),
                                          wander);

            var root = new Repeater(new Sequence(new Succeeder(harvestWheat), new Succeeder(plantWheat)));

            HumanBehavior = new Behavior(root);
        }

        private static void BuildZombieBehaviorTree()
        {
            var interationSequence = new Sequence(new GetHumanAction(), new MoveTowardsTarget());
            var root = new Repeater(interationSequence);
            ZombieBehavior = new Behavior(root);
        }
    }
}