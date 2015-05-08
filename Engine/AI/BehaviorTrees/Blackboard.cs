using System;
using System.Collections.Generic;
using Engine.Entities;

namespace Engine.AI.BehaviorTrees
{
    public class Blackboard : Dictionary<string, object>
    {
        public Blackboard(Entity entity)
        {
            this["subject"] = entity;
            this["Tree"] = new Dictionary<Guid, GoalStatus>();
        }

        public T GetValue<T>(string key)
        {
            if (!ContainsKey(key)) return default(T);

            var value = this[key];

            return (T) value;
        }

        public Dictionary<Guid, GoalStatus> TreeStatus
        {
            get { return (Dictionary<Guid, GoalStatus>) this["Tree"]; }
        }
    }
}

