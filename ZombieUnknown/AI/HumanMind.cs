using Engine.AI;
using Engine.AI.BehaviorTrees;
using ZombieUnknown.AI.BehaviorTrees;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI
{
    public class HumanMind : Mind<Human>
    {
        private readonly Blackboard _blackboard;
        private readonly Behavior _behavior;

        public HumanMind(Human entity) : base(entity)
        {
            entity.SetAnimation("idle");

            _blackboard = new Blackboard(entity);

            _behavior = BehaviorTreeStore.HumanBehavior;
        }

        public override void Think()
        {
            _behavior.Update(_blackboard);
        }
    }
}
