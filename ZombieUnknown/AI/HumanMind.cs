using Engine.AI.BehaviorTrees;
using ZombieUnknown.AI.BehaviorTrees;
using ZombieUnknown.Entities;

namespace ZombieUnknown.AI
{
    public class HumanMind
    {
        private readonly Blackboard _blackboard;
        private readonly Behavior _behavior;

        public HumanMind(Human entity)
        {
            _blackboard = new Blackboard(entity);

            _behavior = BehaviorTreeStore.HumanBehavior;

        }

        public void Think()
        {
            _behavior.Update(_blackboard);
        }
    }
}
