using Engine.Entities;

namespace Engine
{
    public static class GameController
    {
        public static void DeleteEntity(Entity entity)
        {
            GameState.Map.RemoveEntity(entity);
        }

        public static void SpawnEntity(Entity entity)
        {
            GameState.Map.AddEntity(entity);
        }
    }
}
