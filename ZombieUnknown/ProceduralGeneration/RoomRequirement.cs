using System;

namespace ZombieUnknown.ProceduralGeneration
{
    class RoomRequirement
    {
        public String Name;
        public RoomType Type;
        public int Priority;
        public float Area;

        public RoomRequirement(string name, RoomType type, int priority, float area)
        {
            Name = name;
            Type = type;
            Priority = priority;
            Area = area;
        }
    }
}
