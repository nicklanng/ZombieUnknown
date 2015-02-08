using Engine.AI.BehaviorTrees;
using ZombieUnknown.AI.BehaviorTrees;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI
{
    class ZombieMind
    {
        private readonly Blackboard _blackboard;
        private readonly Behavior _behavior;

        public ZombieMind(Zombie entity)
        {
            _blackboard = new Blackboard(entity);

            _behavior = BehaviorTreeStore.ZombieBehavior;
        }

        public void Think()
        {
            _behavior.Update(_blackboard);
        }
    }
}
