using Engine.AI.BehaviorTrees.Actions;
using Engine.AI.BehaviorTrees.Composites;
using Engine.AI.BehaviorTrees.Conditionals;
using Engine.AI.BehaviorTrees.Decorators;

namespace Engine.AI.BehaviorTrees.SubTrees
{
    public class FollowPathSubTree : RepeaterUntilFail
    {
        public FollowPathSubTree() 
            : base(new Sequence(new HasPathConditional(),new IdleAction()))
        {
        }
    }
}
