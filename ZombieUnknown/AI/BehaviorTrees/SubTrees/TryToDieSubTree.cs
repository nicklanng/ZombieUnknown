using Engine.AI.BehaviorTrees.Composites;
using Engine.AI.BehaviorTrees.Decorators;
using ZombieUnknown.AI.BehaviorTrees.Actions;
using ZombieUnknown.AI.BehaviorTrees.Conditionals;

namespace ZombieUnknown.AI.BehaviorTrees.SubTrees
{
    class TryToDieSubTree : Inverter
    {
        public TryToDieSubTree() 
            : base(new Sequence(new ShouldDieConditional(), new DieAction(), new IsDeadConditional()))
        {
            EvaluateEveryTime = true;
        }
    }
}
