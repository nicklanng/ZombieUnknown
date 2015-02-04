using System;
using System.Collections.Generic;
using Engine.Entities;

namespace Engine.AI.BehaviorTrees
{
    public class Blackboard : Dictionary<string, object>
    {
        public Blackboard(Entity entity)
        {
            this["Entity"] = entity;
            this["Tree"] = new Dictionary<Guid, GoalStatus>();
        }

        public Dictionary<Guid, GoalStatus> TreeStatus
        {
            get { return (Dictionary<Guid, GoalStatus>) this["Tree"]; }
        }
    }
}

