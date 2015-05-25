using System.Collections.Generic;

namespace ZombieUnknown.ProceduralGeneration
{
    class RoomRequirementProvider
    {
        private static RoomRequirementProvider _instance;

        public static RoomRequirementProvider Instance
        {
            get { return _instance ?? (_instance = new RoomRequirementProvider()); }
        }

        private readonly List<RoomRequirement> _houseRooms;

        private RoomRequirementProvider()
        {   
            _houseRooms = new List<RoomRequirement>
            {
                new RoomRequirement("Lounge", RoomType.LivingRoom, 1, 1.0f),
                new RoomRequirement("Bathroom", RoomType.Bathroom, 2, 0.4f),
                new RoomRequirement("Kitchen", RoomType.Kitchen, 3, 0.6f),
                new RoomRequirement("Bedroom1", RoomType.Bedroom, 4, 0.8f),
                new RoomRequirement("Bedroom2", RoomType.Bedroom, 4, 0.8f),
            };        
        }

        public List<RoomRequirement> HouseRooms
        {
            get { return _houseRooms; }
        }
    }
}
