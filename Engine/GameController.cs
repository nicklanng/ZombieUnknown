using Engine.Entities;

namespace Engine
{
    public static class GameController
    {
        public static void DeleteEntity(PhysicalEntity entity)
        {
            GameState.Map.RemoveEntity(entity);
        }

        public static void SpawnEntity(PhysicalEntity entity)
        {
            GameState.Map.AddEntity(entity);
        }
    }
}
