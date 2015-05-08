using Engine.AI;
using Engine.AI.BehaviorTrees;
using Engine.AI.UtilityBehaviors;
using ZombieUnknown.Entities.Mobiles;

namespace ZombieUnknown.AI
{
    class HumanMind
    {
        private readonly Human _entity;
        private IUtilityBehavior _currentBehavior;
        private readonly Blackboard _blackboard;
        private Behavior _behavior;

        public HumanMind(Human entity)
        {
            _entity = entity;
            _blackboard = new Blackboard(entity);
        }

        public void Think()
        {
            var utility = UtilityBehaviorRepository.ChooseUtilityBehavior(_entity);

            if (utility == null) return;

            if (utility != _currentBehavior || _behavior.CurrentStatus == GoalStatus.Completed || _behavior.CurrentStatus == GoalStatus.Failed)
            {
                _behavior = new Behavior(utility.BehaviorTree);
                _blackboard.TreeStatus.Clear();
                _currentBehavior = utility;
            }

            _behavior.Update(_blackboard);
        }
    }
}
